using Common.Helpers;
using Common.Models;

namespace Common.FitnessTests.Parts;

public class DistanceFromStartFitnessDecorator : BaseFitnessDecorator {
	protected override double ScorePart(Creature creature) => creature.Position.CalculateDistanceBetweenPositions(creature.StartPosition);
	public DistanceFromStartFitnessDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
