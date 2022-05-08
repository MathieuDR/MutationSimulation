namespace Common.Models; 

public class World {
	public World(int width, int height, Creature[] creatures, ulong tick = 0) {
		Width = width;
		Height = height;
		Creatures = creatures;
		Tick = tick;
	}
	public int Width { get; init; }
	public int Height { get; init; }
	public Creature[] Creatures { get; init; }
	public ulong Tick { get; private set; }
	
	public void Deconstruct(out int width, out int height, out Creature[] creatures, out ulong tick) {
		width = Width;
		height = Height;
		creatures = Creatures;
		tick = Tick;
	}
	
	public void Simulate() {
		Tick++;
		foreach (var creature in Creatures) {
			creature.Simulate(this);
		}
	}
}
