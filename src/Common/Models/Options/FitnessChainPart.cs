using Common.FitnessTests.Parts;

namespace Common.Models.Options;

public struct FitnessChainPart {
	public FitnessChainPart(FitnessPart part, double multiplier) {
		Part = part;
		Multiplier = multiplier;
	}
	public FitnessPart Part { get; init; }
	public double Multiplier { get; init; }
}
