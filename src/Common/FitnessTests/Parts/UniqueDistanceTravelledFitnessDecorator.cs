using Common.Models;

namespace Common.FitnessTests.Parts;

public class UniqueDistanceTravelledFitnessDecorator : BaseFitnessDecorator {
	protected override double ScorePart(Creature creature) => creature.UniquePositions.Count() * creature.Speed;
	public UniqueDistanceTravelledFitnessDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
