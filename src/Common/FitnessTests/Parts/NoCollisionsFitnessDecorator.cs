using Common.Models;

namespace Common.FitnessTests.Parts;

public class NoCollisionsFitnessDecorator : BaseFitnessDecorator {
	
	protected override double ScorePart(Creature creature) => -creature.Collisions;
	public NoCollisionsFitnessDecorator(IFitnessPart nextPart, double multiplier) : base(nextPart, multiplier) { }
}
