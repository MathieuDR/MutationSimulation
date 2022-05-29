using Common.Models;
using Graphics.Helpers;
using SkiaSharp;

namespace Graphics;

public class WorldRenderMachine {
	private readonly int _multiplier;
	private readonly int _wallWidth;
	private readonly string _fileName;
	private readonly string _filePath;
	private readonly SKEncodedImageFormat _format;
	private readonly int _quality;
	private ulong _frameCount;

	public WorldRenderMachine(string path, string filename, int wallWidth = 4, SKEncodedImageFormat format = SKEncodedImageFormat.Png,
		int quality = 100, int multiplier = 1) {
		_wallWidth = wallWidth;
		_format = format;
		_quality = quality;
		_filePath = path;
		_fileName = filename;
		_multiplier = multiplier;
	}

	public string RenderWorld(World world) {
		using var surface =
			SKSurface.Create(new SKImageInfo(World.Width * _multiplier , World.Height * _multiplier ));
		using var canvas = surface.Canvas;

		canvas.Clear(SKColors.White);

		DrawBlobs(canvas, world.Creatures);
		DrawWalls(canvas, SKColors.DarkRed, world.Walls);

		return SaveFrame(surface);
	}

	private readonly ulong[] _ids = new ulong[]{};

	private void DrawBlobs(SKCanvas canvas, Creature[] worldBlobs) {
		foreach (var creature in worldBlobs) {
			if(!_ids.Any() || _ids.Contains(creature.Id)) 
				creature.Draw(canvas, GetImagePosition, GetPixelSize);
		}
	}
	
	private int GetPixelSize(int size) => size * _multiplier;

	private (int X, int Y) GetImagePosition(Vector vector) =>
		(vector.PixelX * _multiplier , vector.PixelY * _multiplier );

	private string SaveFrame(SKSurface surface) {
		var path = GetCurrentPath();
		surface.SaveToPath(path, _format, _quality);
		_frameCount++;
		return path;
	}

	private void DrawWalls(SKCanvas canvas, SKColor color, Line[] walls) {
		var borderPaint = new SKPaint {
			Style = SKPaintStyle.Stroke,
			StrokeWidth = _wallWidth,
			Color = color
		};
		
		var halfWidth = _wallWidth / 2;

		// Draw the world.
		foreach (var wall in walls) {
			canvas.DrawLine((float)wall.StartPoint.X, (float)wall.StartPoint.Y , (float)wall.EndPoint.X, (float)wall.EndPoint.Y,
				borderPaint);
		}
	}


	private string GetCurrentPath() => $"{_filePath}/{_fileName}_{_frameCount.ToString("D8")}.{_format.ToString().ToLower()}";
}
