using Common.Models;
using Common.Models.Enums;

namespace Common.FitnessTests.Parts;

public class LocationComboDecorator : BaseFitnessDecorator {
	protected override double ScorePart(Creature creature) => creature.GetValueMetric(ValueMetric.MaxCombo);
	public LocationComboDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
