using Common.Models;

namespace Common.FitnessTests.Parts;

public class DistanceTravelledFitnessDecorator : BaseFitnessDecorator {
	
	protected override double ScorePart(Creature creature) => creature.Distance;
	public DistanceTravelledFitnessDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
