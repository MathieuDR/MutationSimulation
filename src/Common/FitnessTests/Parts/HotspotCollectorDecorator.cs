using Common.Models;
using Common.Models.Enums;

namespace Common.FitnessTests.Parts;

public class HotspotCollectorDecorator : BaseFitnessDecorator {
	protected override double ScorePart(Creature creature) => creature.CountListMetric(ListMetric.VisitedHotspots);
	public HotspotCollectorDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
