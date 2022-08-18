using Common.Models.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using SkiaSharp;

namespace Graphics.RenderEngines;

public class SilkWindow {
	private readonly ILogger<SilkWindow> _logger;
	private readonly IHostApplicationLifetime _lifetime;
	private readonly RenderOptions _renderOptions;
	private readonly WorldOptions _worldOptions;
	private IWindow? _window;
	private SKSurface? _surface;
	private bool _isReady = false;

		
	public SKSurface SKSurface {
		get { return _surface ?? throw new NullReferenceException(nameof(_surface)); }
	}
	
	public GRContext? GrContext { get; private set; }
	
	// Silk.NET-Skia interop variables
	// SKSurface surface = null!;
	// SKCanvas canvas = null!;

	private IWindow Window => _window ?? throw new NullReferenceException(nameof(_window));
	

	public SilkWindow(ILogger<SilkWindow> logger, IOptionsSnapshot<RenderOptions> renderOptions, IOptionsSnapshot<WorldOptions> worldOptions, IHostApplicationLifetime lifetime) {
		_logger = logger;
		_lifetime = lifetime;
		_renderOptions = renderOptions.Value;
		_worldOptions = worldOptions.Value;
	}

	public void Create() {
		// create size
		Vector2D<int> size = new(_worldOptions.Width * _renderOptions.PixelMultiplier, _worldOptions.Height* _renderOptions.PixelMultiplier);

		// Create the Silk.NET window
		var options = WindowOptions.Default with {
			Size = size,
			Title = "Mutating is fun",
			PreferredStencilBufferBits = 8,
			PreferredBitDepth = new Vector4D<int>(8),
			FramesPerSecond = 30,
			//IsEventDriven = true
		};

		_window = Silk.NET.Windowing.Window.Create(options);
		_window.Load += OnWindowOnLoad;
	}

	private void OnWindowOnLoad() {
		_logger.LogInformation("Window loaded");
		_surface = GetRenderTarget();
		_isReady = true;
	}

	private SKSurface GetRenderTarget() {
		_logger.LogInformation("Creating render target");
		var renderTarget = new GRBackendRenderTarget(_worldOptions.Width* _renderOptions.PixelMultiplier, _worldOptions.Height* _renderOptions.PixelMultiplier, 0, 8, new GRGlFramebufferInfo(0, 32856));
		
		var grGlInterface = GRGlInterface.Create(name => !name.StartsWith("egl") && Window.GLContext!.TryGetProcAddress(name, out var addr) ? addr : 0);
		grGlInterface.Validate();
		GrContext = GRContext.CreateGl(grGlInterface);
		return SKSurface.Create(GrContext, renderTarget, GRSurfaceOrigin.BottomLeft, SKColorType.Rgba8888);
	}

	public void HookRenderer(Action<double> hook) {
		Window.Render += hook;
	}

	public void UnhookRenderer(Action<double> hook) {
		Window.Render -= hook;
	}

	public async Task WaitOnLoad() {
		int i = 0;
		while (!_isReady) {
			await Task.Delay(100);
			i++;
			if(i > 100) {
				// 10 seconds
				throw new Exception("Window failed to load");
			}
		}
	}

	public void Start() {
		Window.Closing += () => {
			_logger.LogInformation("Window closing");
			_lifetime.StopApplication();
		};
		
		if(!Window.IsInitialized)
			Window.Run();
	}

	public void Render() {
		// Window.DoRender();
		// //Window.DoUpdate();
		// Window.ContinueEvents();
	}
}
