namespace Common.Models;

public readonly record struct Vector(double X, double Y) {
	public Vector(Random random, int worldWidth, int worldHeight, int diameter) : this(random.Next(diameter, worldWidth - diameter), random.Next(diameter, worldHeight - diameter)){ }

	public int PixelX => (int)Math.Round(X);
	public int PixelY => (int)Math.Round(Y);
};