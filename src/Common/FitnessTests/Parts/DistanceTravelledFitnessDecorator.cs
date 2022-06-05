using Common.Models;
using Common.Models.Enums;

namespace Common.FitnessTests.Parts;

public class DistanceTravelledFitnessDecorator : BaseFitnessDecorator {
	
	protected override double ScorePart(Creature creature) => creature.GetValueMetric(ValueMetric.Distance);
	public DistanceTravelledFitnessDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
