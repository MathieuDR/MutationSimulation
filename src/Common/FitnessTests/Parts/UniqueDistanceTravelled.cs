using Common.Models;

namespace Common.FitnessTests.Parts;

public class UniqueDistanceTravelled : IFitnessPart {
	public double Score(Creature creature) =>
		creature.UniquePositions.Count() * creature.Speed * FitnessParams.UniqueDistanceMultiplier;
}