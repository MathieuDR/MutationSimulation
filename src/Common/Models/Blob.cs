namespace Common.Models; 

public record Blob(Position Position, Velocity Velocity, int Diameter, Color Color) {
	public Blob(Random random, int maxVelocity, int maxDiameter, int worldWidth, int worldHeight) : 
		this( new(0, 0), new Velocity(random, maxVelocity), random.Next(5, Math.Max(6, maxDiameter)), new Color(random)) {
		Position = new Position(random, worldWidth, worldHeight, Diameter);
	}

	public int Radius => Diameter / 2;
}

public record Position(double X, double Y) {
	public Position(Random random, int worldWidth, int worldHeight, int diameter) : this(random.Next(diameter, worldWidth - diameter), random.Next(diameter, worldHeight - diameter)){ }

	public int PixelX => (int)Math.Round(X);
	public int PixelY => (int)Math.Round(Y);
};

public record Velocity(double X, double Y) {
	public Velocity(Random random, int maxVelocity) : this((random.NextDouble() - random.NextDouble()) * maxVelocity, (random.NextDouble() - random.NextDouble())  * maxVelocity){ }
};

public record Color(byte R, byte G, byte B) {
	public Color(Random random) : this((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)) { }
}