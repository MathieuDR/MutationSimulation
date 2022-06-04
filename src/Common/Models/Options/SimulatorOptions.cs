using System.ComponentModel.DataAnnotations;

namespace Common.Models.Options;

public record SimulatorOptions : ConfigurationOptions {
	public const string SectionName = "Simulator";

	public SimulatorOptions() { }

	public SimulatorOptions(int? generations, int steps, bool validateStartPositions) {
		Generations = generations;
		Steps = steps;
		ValidateStartPositions = validateStartPositions;
	}

	public int? Generations { get; init; } = 5000;

	[Range(10, 20000)]
	public int Steps { get; init; } = 500;

	public bool ValidateStartPositions { get; init; } = true;

	public double MutationRate { get; init; }

	public void Deconstruct(out int? generations, out int steps, out bool validateStartPositions) {
		generations = Generations;
		steps = Steps;
		validateStartPositions = ValidateStartPositions;
	}
}
