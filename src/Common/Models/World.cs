using Common.Models.Enums;

namespace Common.Models;

public class World {
	public static int Height;
	public static int Width;
	public World(int width, int height, Creature[] creatures, Line[] walls, ulong tick = 0) {
		Width = width;
		Height = height;
		Creatures = creatures;
		Walls = walls;
		Tick = tick;
	}

	// public int Width { get; init; }
	// public int Height { get; init; }
	public Creature[] Creatures { get; init; }
	public ulong Tick { get; private set; }
	public Line[] Walls { get; init; }

	public void Deconstruct(out int width, out int height, out Creature[] creatures, out ulong tick) {
		width = Width;
		height = Height;
		creatures = Creatures;
		tick = Tick;
	}

	public void NextTick() {
		Tick++;
		foreach (var creature in Creatures) {
			creature.Simulate(this);
		}
	}
	public Line GetClosestWallInDirection(Vector vector, Direction direction) => Walls.First();
}
