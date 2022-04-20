using SkiaSharp;

namespace Graphics.Models;

public record World {
	public World(int Width, int Height, int Border = 2) {
		this.Width = Width;
		this.Height = Height;
		this.Border = Border;
	}

	public int Width { get; init; }
	public int Height { get; init; }
	public int Border { get; init; }

	public SKSurface RenderSurface() {
		var surface = SKSurface.Create(new SKImageInfo(Width + Border, Height + Border));
		using var canvas = surface.Canvas;

		canvas.Clear(SKColors.White);
		DrawBorder(canvas, SKColors.DarkRed);


		return surface;
	}

	private void DrawBorder(SKCanvas canvas, SKColor color) {
		var borderPaint = new SKPaint {
			Style = SKPaintStyle.Stroke,
			StrokeWidth = Border,
			Color = color
		};

		// Draw the world.
		var halvedBorder = Border / 2;
		canvas.DrawRect(new SKRect(halvedBorder, halvedBorder, Width + halvedBorder, Height + halvedBorder), borderPaint);
	}

	public void Deconstruct(out int Width, out int Height, out int Border) {
		Width = this.Width;
		Height = this.Height;
		Border = this.Border;
	}
}
