using Common.Models;

namespace Common.FitnessTests.Parts;

public class NoCollisionsFitnessParts : IFitnessPart {
	public double Score(Creature creature) => creature.Collisions * FitnessParams.CollisionMultiplier;
}