using Common.Models.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using SkiaSharp;

namespace Graphics.Renderer;

// example: https://github.com/davidwengier/Trains.NET/blob/main/src/SilkTrains/Program.cs
public class SilkRenderer {
	private readonly ILogger<SilkRenderer> _logger;
	private readonly IHostApplicationLifetime _lifetime;
	private readonly RenderOptions _renderOptions;
	private readonly WorldOptions _worldOptions;
	private IWindow? _window;
	private SKSurface? _surface;

	public SKSurface SKSurface {
		get { return _surface ??= GetRenderTarget(); }
	}
	
	public GRContext? GrContext { get; private set; }
	
	// Silk.NET-Skia interop variables
	// SKSurface surface = null!;
	// SKCanvas canvas = null!;

	private IWindow Window => _window ??= CreateWindow();

	public SilkRenderer(ILogger<SilkRenderer> logger, IOptionsSnapshot<RenderOptions> renderOptions, IOptionsSnapshot<WorldOptions> worldOptions, IHostApplicationLifetime lifetime) {
		_logger = logger;
		_lifetime = lifetime;
		_renderOptions = renderOptions.Value;
		_worldOptions = worldOptions.Value;
	}

	public IWindow CreateWindow() {
		// create size
		Vector2D<int> size = new(_worldOptions.Width * _renderOptions.PixelMultiplier, _worldOptions.Height* _renderOptions.PixelMultiplier);

		// Create the Silk.NET window
		var options = WindowOptions.Default with {
			Size = size,
			Title = "Mutating is fun",
			PreferredStencilBufferBits = 8,
			PreferredBitDepth = new Vector4D<int>(8),
			FramesPerSecond = 30
		};

		return Silk.NET.Windowing.Window.Create(options);
	}

	private SKSurface GetRenderTarget() {
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

	public void StartWindow() {
		Window.Closing += () => {
			_logger.LogInformation("Window closing");
			_lifetime.StopApplication();
		};
		
		if(!Window.IsInitialized)
			Window.Run();
	}

	public void Render() {
		Window.DoRender();
	}
}
