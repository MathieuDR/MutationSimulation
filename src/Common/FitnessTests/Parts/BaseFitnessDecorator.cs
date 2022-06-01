using Common.Models;

namespace Common.FitnessTests.Parts;

public abstract class BaseFitnessDecorator : IFitnessPart {
	protected BaseFitnessDecorator(IFitnessPart nextPart, double multiplier) {
		NextPart = nextPart;
		Multiplier = multiplier;
	}

	private IFitnessPart NextPart { get; }
	protected double Multiplier { get; }
	public double Score(Creature creature) => ScorePart(creature) * Multiplier + NextPart?.Score(creature) ?? 0;
	protected abstract double ScorePart(Creature creature);
}
