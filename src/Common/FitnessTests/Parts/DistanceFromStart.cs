using Common.Helpers;
using Common.Models;

namespace Common.FitnessTests.Parts;

public class DistanceFromStart : IFitnessPart {
	public double Score(Creature creature) => creature.Position.CalculateDistanceBetweenPositions(creature.StartPosition) *
		FitnessParams.DistanceFromStartMultiplier;
}