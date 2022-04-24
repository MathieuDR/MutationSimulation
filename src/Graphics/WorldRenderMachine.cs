using Common.Interfaces;
using Common.Models;
using Graphics.Helpers;
using LanguageExt;
using SkiaSharp;

namespace Graphics;

public class WorldRenderMachine {
	private readonly int _multiplier;
	private readonly int _borderWidth;
	private readonly string _fileName;
	private readonly string _filePath;
	private readonly SKEncodedImageFormat _format;
	private readonly int _quality;
	private readonly Random _random;
	private ulong _frameCount;

	public WorldRenderMachine(string path, string filename, int borderWidth = 5, SKEncodedImageFormat format = SKEncodedImageFormat.Png,
		int quality = 100, int? randomSeed = null, int multiplier = 2) {
		_borderWidth = borderWidth;
		_format = format;
		_quality = quality;
		_filePath = path;
		_fileName = filename;
		_multiplier = multiplier;

		_random = randomSeed.HasValue ? new Random(randomSeed.Value) : new Random();
	}

	public Option<string> RenderWorld(World world) {
		using var surface =
			SKSurface.Create(new SKImageInfo(world.Width * _multiplier + _borderWidth * 2, world.Height * _multiplier + _borderWidth * 2));
		using var canvas = surface.Canvas;

		canvas.Clear(SKColors.White);

		DrawBlobs(canvas, world.Blobs);
		DrawBorder(canvas, SKColors.DarkRed, world.Width, world.Height);

		return SaveFrame(surface);
	}

	private void DrawBlobs(SKCanvas canvas, ICreature[] worldBlobs) {
		foreach (var creature in worldBlobs) {
			creature.Draw(canvas, GetImagePosition, GetPixelSize);
		}
	}
	
	private int GetPixelSize(int size) => size * _multiplier;

	private (int X, int Y) GetImagePosition(Position position) =>
		(position.PixelX * _multiplier + _borderWidth, position.PixelY * _multiplier + _borderWidth);

	private SKColor GetRandomColor() => new((byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255));

	private Option<string> SaveFrame(SKSurface surface) {
		var path = GetCurrentPath();
		surface.SaveToPath(path, _format, _quality);
		_frameCount++;
		return new Option<string>(new[] { path });
	}

	private void DrawBorder(SKCanvas canvas, SKColor color, int width, int height) {
		var borderPaint = new SKPaint {
			Style = SKPaintStyle.Stroke,
			StrokeWidth = _borderWidth,
			Color = color
		};

		// Draw the world.
		var halvedBorder = _borderWidth / 2;
		canvas.DrawRect(new SKRect(halvedBorder, halvedBorder, width * _multiplier + halvedBorder + _borderWidth, height * _multiplier + halvedBorder + _borderWidth),
			borderPaint);
	}


	private string GetCurrentPath() => $"{_filePath}/{_fileName}_{_frameCount.ToString("D8")}.{_format.ToString().ToLower()}";
}
