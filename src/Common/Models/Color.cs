namespace Common.Models;

public record Color(byte R, byte G, byte B) {
	public Color(Random random) : this((byte)random.Next(0, 255), (byte)random.Next(0, 255), (byte)random.Next(0, 255)) { }
}