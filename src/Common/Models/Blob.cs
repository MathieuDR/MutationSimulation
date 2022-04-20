namespace Common.Models; 

public record Blob(Position Position, Velocity Velocity, int Diameter, Color Color) {
	public int Radius => Diameter / 2;
}

public record Position(int X, int Y);
public record Velocity(int X, int Y);

public record Color(byte R, byte G, byte B) {
	public Color(Random random) : this((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)) { }
}