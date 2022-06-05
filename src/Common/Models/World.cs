namespace Common.Models;

public class World {
	public static int Height;
	public static int Width;
	public World(int width, int height, Creature[] creatures) {
		Width = width;
		Height = height;
		Creatures = creatures;
	}

	public Creature[] Creatures { get; init; }
	public Line[] Walls { get; init; }

	public Hotspot[] Hotspots { get; init; }

	public void Deconstruct(out int width, out int height, out Creature[] creatures) {
		width = Width;
		height = Height;
		creatures = Creatures;
	}
}
