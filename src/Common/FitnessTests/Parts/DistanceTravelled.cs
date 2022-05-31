using Common.Models;

namespace Common.FitnessTests.Parts;

public class DistanceTravelled : IFitnessPart {
	public double Score(Creature creature) => creature.Distance * FitnessParams.DistanceTravelledMultiplier;
}