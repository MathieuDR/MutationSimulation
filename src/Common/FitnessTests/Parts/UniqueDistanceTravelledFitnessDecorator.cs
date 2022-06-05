using Common.Models;
using Common.Models.Enums;

namespace Common.FitnessTests.Parts;

public class UniqueDistanceTravelledFitnessDecorator : BaseFitnessDecorator {
	protected override double ScorePart(Creature creature) => creature.CountListMetric(ListMetric.Visited);
	public UniqueDistanceTravelledFitnessDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
