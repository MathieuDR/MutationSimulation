using Common.Interfaces;
using SkiaSharp;

namespace Common.Models;

public record Creature(Genome Genome, Position Position, int Radius, Color Color) : ICreature {
	public int Age { get;init; } = 0;
	public ICreature Simulate(World world) {
		return this with {Age = Age + 1};
	}

	public void Draw(SKCanvas canvas, Func<Position, (int X, int Y)> calculatePixelPosition, Func<int, int> pixelSize) {
		var fillPaint = new SKPaint {
			Style = SKPaintStyle.Fill,
			Color = new SKColor(Color.R, Color.G, Color.B)
		};
		
		var pixelPosition = calculatePixelPosition(Position);

		canvas.DrawCircle(pixelPosition.X, pixelPosition.Y , pixelSize(Radius) , fillPaint);
	}
}
