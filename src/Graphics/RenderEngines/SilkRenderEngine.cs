using Common.Helpers;
using Common.Models.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using SkiaSharp;

namespace Graphics.RenderEngines; 

public class SilkRenderEngine : IRenderEngine, IDisposable{
    private readonly ILogger<SilkRenderEngine> _logger;
    private readonly IHostApplicationLifetime _applicationLifetime;
    private readonly RenderOptions _renderOptions;
    private readonly WorldOptions _worldOptions;
    private IWindow? _window;
    private SKSurface? _surface;
    private bool _isReady = false;
    private GRContext _grContext;
    
    private object renderLock = new ();

    public SilkRenderEngine(ILogger<SilkRenderEngine> logger, IOptionsSnapshot<RenderOptions> renderOptions, IOptionsSnapshot<WorldOptions> worldOptions, IHostApplicationLifetime applicationLifetime) {
        _logger = logger;
        
        _applicationLifetime = applicationLifetime;
        _renderOptions = renderOptions.Value;
        _worldOptions = worldOptions.Value;
    }
    
    public async Task Initialize() {
        if (_window is not null) {
            return;
        }
        
        Vector2D<int> size = new(_worldOptions.Width * _renderOptions.PixelMultiplier, _worldOptions.Height* _renderOptions.PixelMultiplier);

        // Create the Silk.NET window
        var options = WindowOptions.Default with {
            Size = size,
            Title = "Mutating is fun",
            PreferredStencilBufferBits = 8,
            PreferredBitDepth = new Vector4D<int>(8),
            FramesPerSecond = 10,
        };

        _window = Window.Create(options);
        _window.Load += OnWindowOnLoad;
        _window.Closing += OnWindowOnClosing;
        _window.Render += SilkRender;

    }

    private void OnWindowOnClosing() {
        _logger.LogInformation("Window closing");
        _applicationLifetime.StopApplication();
    }

    private void OnWindowOnLoad() {
        _logger.LogInformation("Window loaded");
        CreateRenderTarget();
        _isReady = true;
    }
    
    private void CreateRenderTarget() {
        _logger.LogInformation("Creating render target");
        var renderTarget = new GRBackendRenderTarget(_worldOptions.Width* _renderOptions.PixelMultiplier, _worldOptions.Height* _renderOptions.PixelMultiplier, 0, 8, new GRGlFramebufferInfo(0, 32856));
		
        var grGlInterface = GRGlInterface.Create(name => !name.StartsWith("egl") && _window!.GLContext!.TryGetProcAddress(name, out var addr) ? addr : 0);
        grGlInterface.Validate();
        _grContext = GRContext.CreateGl(grGlInterface);

        // Do I need to lock this, here?
        lock (renderLock) {
            _surface = SKSurface.Create(_grContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
        }
    }

    public async Task SaveFrame(string path) {
        if(_surface is null) {
            return;
        }
        
        SKImage? image = null;
        lock (renderLock) {
           image = _surface.Snapshot();
        }

        // Wrap in run to create non blocking task
        await Task.Run(() => {
            if (image is null) {
                return;
            }

            FileHelper.EnsurePath(path);
            using var stream = File.OpenWrite(path);
            using var data = image.Encode(SKEncodedImageFormat.Png, 100);
            data.SaveTo(stream);
            image.Dispose();
        });
    }

    public void SilkRender(double deltaTime) {
        lock (renderLock) {
            if (_surface is null) {
                return;
            }
            
            Render?.Invoke(_surface.Canvas);
        }
    }

    public event Action<SKCanvas>? Render;

    public void Dispose() {
        _window?.Dispose();
        _surface?.Dispose();
        _grContext.Dispose();
    }
}