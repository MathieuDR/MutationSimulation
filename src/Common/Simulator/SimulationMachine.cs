using Common.Models;

namespace Common.Simulator; 

public static class SimulationMachine {
	public static World Tick(World world) {
		return world with { Tick = world.Tick + 1 };
	}
}
