namespace Common.Models;

public record Position(double X, double Y) {
	public Position(Random random, int worldWidth, int worldHeight, int diameter) : this(random.Next(diameter, worldWidth - diameter), random.Next(diameter, worldHeight - diameter)){ }

	public int PixelX => (int)Math.Round(X);
	public int PixelY => (int)Math.Round(Y);
};