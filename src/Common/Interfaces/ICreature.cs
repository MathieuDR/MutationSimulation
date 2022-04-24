using Common.Models;
using SkiaSharp;

namespace Common.Interfaces; 

public interface ICreature {
	public ICreature Simulate(World world);
	public void Draw(SKCanvas canvas, Func<Position, (int X, int Y)> calculatePixelPosition, Func<int, int> pixelSize);
	public Position Position { get; }
}
