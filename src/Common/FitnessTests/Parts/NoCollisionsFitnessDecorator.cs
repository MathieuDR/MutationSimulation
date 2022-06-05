using Common.Models;
using Common.Models.Enums;

namespace Common.FitnessTests.Parts;

public class NoCollisionsFitnessDecorator : BaseFitnessDecorator {
	
	protected override double ScorePart(Creature creature) => -creature.GetValueMetric(ValueMetric.Collisions);
	public NoCollisionsFitnessDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
