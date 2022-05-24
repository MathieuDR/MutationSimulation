using System.ComponentModel.DataAnnotations;
using Common.Models.Enums;

namespace Common.Models.Options;

public record BrainOptions : ConfigurationOptions {
	public const string SectionName = "Brain";

	public BrainOptions() { }

	public BrainOptions(int startNeurons, int maxInternalNeuron, ActivationFunction internalActivationFunction) {
		StartNeurons = startNeurons;
		MaxInternalNeuron = maxInternalNeuron;
		InternalActivationFunction = internalActivationFunction;
	}

	[Range(1, 200)]
	public int StartNeurons { get; init; } = 20;

	[Range(1, 200)]
	public int MaxInternalNeuron { get; init; } = 20;

	public ActivationFunction InternalActivationFunction { get; init; } = ActivationFunction.ReLu;

	public void Deconstruct(out int startNeurons, out int maxInternalNeuron, out ActivationFunction internalActivationFunction) {
		startNeurons = StartNeurons;
		maxInternalNeuron = MaxInternalNeuron;
		internalActivationFunction = InternalActivationFunction;
	}

	public override bool Validate(out ICollection<ValidationResult> results) {
		var isValid = base.Validate(out results);

		if (MaxInternalNeuron < StartNeurons) {
			isValid = false;
			results.Add(new ValidationResult($"{nameof(MaxInternalNeuron)} must be higher then {nameof(StartNeurons)}",
				new[] { nameof(MaxInternalNeuron), nameof(StartNeurons) }));
		}

		return isValid;
	}
}
