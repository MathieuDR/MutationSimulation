using System.Security.AccessControl;
using Common.Helpers;
using Common.Models;
using Common.Models.Options;
using Graphics.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Graphics;

public class RenderMachine {
	private readonly GenerationContext _context;
	private readonly ILogger<RenderMachine> _logger;
	private readonly GifRenderer _gifRenderer;
	private readonly RenderOptions _renderOptions;

	private string? _path;

	private readonly List<string>? _frames;
	public RenderMachine(GenerationContext context, ILogger<RenderMachine> logger, IOptionsSnapshot<RenderOptions> renderOptions, GifRenderer gifRenderer) {
		_context = context;
		_logger = logger;
		_gifRenderer = gifRenderer;
		_renderOptions = renderOptions.Value;
		if (!context.RenderFrames) {
			_logger.LogTrace("Not rendering generation {gen}", _context.Generation);
			return;
		}
		
		_frames = context.RenderGif ? new() : null;
		_path = Path.Combine(_context.BaseOutputPath, "frames");
		_logger.LogTrace("Rendering generation {gen} in {path}", _context.Generation, _path);
		FileHelper.EnsurePath(_path);
	}

	public async Task RenderFrame() {
		if(!_context.RenderFrames || _context.Tick % _renderOptions.TicksPerFrame != 0) {
			return;
		}
		
		using var surface =
			SKSurface.Create(new SKImageInfo(World.Width * _renderOptions.PixelMultiplier , World.Height * _renderOptions.PixelMultiplier ));
		using var canvas = surface.Canvas;

		canvas.Clear(SKColors.White);

		DrawBlobs(canvas, _context.Creatures);
		DrawWalls(canvas, SKColors.DarkRed, _context.World.Walls);
		await Task.Run(() => SaveFrame(surface));
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

	
}
