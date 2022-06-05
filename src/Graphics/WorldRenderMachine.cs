using Common.Helpers;
using Common.Models;
using Common.Models.Options;
using Graphics.Helpers;
using Graphics.Renderer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Graphics;

public class RenderMachine : IDisposable{
	private readonly GenerationContext _context;
	private readonly ILogger<RenderMachine> _logger;
	private readonly GifRenderer _gifRenderer;
	private readonly SilkRenderer _silkRenderer;
	private readonly RenderOptions _renderOptions;

	private string? _path;

	private readonly List<string>? _frames;
	public RenderMachine(GenerationContext context, ILogger<RenderMachine> logger, IOptionsSnapshot<RenderOptions> renderOptions, GifRenderer gifRenderer, SilkRenderer silkRenderer) {
		_context = context;
		_logger = logger;
		_gifRenderer = gifRenderer;
		_silkRenderer = silkRenderer;
		_renderOptions = renderOptions.Value;
		
		if (!context.RenderFrames) {
			_logger.LogTrace("Not rendering generation {gen}", _context.Generation);
			return;
		}

		if (_context.Generation == 0) {
			try {
				_silkRenderer.CreateWindow();
				
				Task.Run(() => _silkRenderer.StartWindow());
			} catch (Exception e) {
				_logger.LogError(e, "Failed to create window");
			}
		}
		
		_silkRenderer.HookRenderer(Render);
		_frames = context.RenderGif ? new() : null;
		_path = Path.Combine(_context.BaseOutputPath, "frames");
		_logger.LogTrace("Rendering generation {gen} in {path}", _context.Generation, _path);
		
		
		
		FileHelper.EnsurePath(_path);
	}

	private void Render(double delta) {
		try {
			var canvas = _silkRenderer.SKSurface.Canvas;

			canvas.Clear(SKColors.White);

			DrawBlobs(canvas, _context.Creatures);
			DrawWalls(canvas, SKColors.DarkRed, _context.World.Walls);
			DrawHotspots(canvas, SKColors.OrangeRed, _context.World.Hotspots);
			canvas.Flush();
		}catch (Exception e) {
			_logger.LogError(e, "Failed to render generation {gen}", _context.Generation);
		}
	}

	private void DrawHotspots(SKCanvas canvas, SKColor color, Hotspot[] worldHotspots) {
		var fillPaint = new SKPaint {
			Style = SKPaintStyle.Fill,
			Color = color.WithAlpha(128)
		};

		foreach (var worldHotspot in worldHotspots) {
			var pos = GetImagePosition(worldHotspot.Position);
			var size = (float)(worldHotspot.Radius * _renderOptions.PixelMultiplier);
			canvas.DrawCircle(pos.X, pos.Y, size, fillPaint);
		}
	}

	public async Task RenderFrame() {
		if(!_context.RenderFrames || _context.Tick % _renderOptions.TicksPerFrame != 0) {
			return;
		}

		//_silkRenderer.Render();

		//
		// var canvas = _silkRenderer.SKSurface.Canvas;
		//
		// canvas.Clear(SKColors.White);
		//
		// DrawBlobs(canvas, _context.Creatures);
		// DrawWalls(canvas, SKColors.DarkRed, _context.World.Walls);
		//await Task.Run(() => SaveFrame(surface));
	}

	public Task QueueGifRender() {
		if (!_context.RenderGif) {
			return Task.CompletedTask;
		}
		
		var path =  Path.Combine(_renderOptions.OutputDirectory, "gifs", $"gen_{_context.Generation:D4}.gif");
		
		_gifRenderer.StartGifRender(_frames!.ToArray(), path);
		return Task.CompletedTask;
	}
	
	private Task SaveFrame(SKSurface surface) {
		var frame = _context.Tick / _renderOptions.TicksPerFrame;
		var filePath = Path.Combine(_path!, frame.ToString("D5") + ".png");
		surface.SaveToPath(filePath, SKEncodedImageFormat.Png, 100);

		if (_context.RenderGif) {
			_frames!.Add(filePath);
		}

		return Task.CompletedTask;
	}

	private void DrawBlobs(SKCanvas canvas, Creature[] worldBlobs) {
		foreach (var creature in worldBlobs) {
			creature.Draw(canvas, GetImagePosition, GetPixelSize);
		}
	}
	
	private void DrawWalls(SKCanvas canvas, SKColor color, Line[] walls) {
		var borderPaint = new SKPaint {
			Style = SKPaintStyle.Stroke,
			StrokeWidth = _renderOptions.WallWidth,
			Color = color
		};

		foreach (var wall in walls) {
			canvas.DrawLine((float)wall.StartPoint.X, (float)wall.StartPoint.Y , (float)wall.EndPoint.X, (float)wall.EndPoint.Y,
				borderPaint);
		}
	}
	
	private int GetPixelSize(int size) => size * _renderOptions.PixelMultiplier;

	private (int X, int Y) GetImagePosition(Vector vector) => (vector.PixelX * _renderOptions.PixelMultiplier , vector.PixelY * _renderOptions.PixelMultiplier );

	public void Dispose() {
		_silkRenderer.UnhookRenderer(Render);
	}
}
