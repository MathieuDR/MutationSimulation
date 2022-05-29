using System.ComponentModel.DataAnnotations;
using Common.Models.Enums;
using Common.Models.Genetic.Components.Neurons;

namespace Common.Models.Options;

public record BrainOptions : ConfigurationOptions {
	public const string SectionName = "Brain";

	public BrainOptions() { }

	public BrainOptions(int startNeurons, int maxNeurons, ActivationFunction internalActivationFunction) {
		StartNeurons = startNeurons;
		MaxNeurons = maxNeurons;
		InternalActivationFunction = internalActivationFunction;
	}

	[Range(1, 200)]
	public int StartNeurons { get; init; } = 15;

	[Range(1, 199)]
	public int? MinStartNeurons { get; init; } = 5;

	[Range(1, 200)]
	public int MaxNeurons { get; init; } = 20;

	[Range(0, 1)]
	public double InternalRate { get; init; } = 0.33d;
	
	[Range(0, 1)]
	public double InputRate { get; init; } = 0.5d;

	[Range(1, 100)]
	public int MaxOutgoingConnectionsPerNeuron { get; init; } = 50;
	
	[Range(1, 100)]
	public int StartingConnectionsPerNeuron { get; init; } = 10;
	[Range(1, 100)]
	public int? MinStartingConnectionsPerNeuron { get; init; } = 1;

	public InputType[] EnabledInputNeurons { get; init; } = new[] {
		InputType.Age,
		InputType.Oscillator,
		InputType.LookBack,
		InputType.LookFront,
		InputType.LookLeft,
		InputType.LookRight,
		InputType.Speed,
		InputType.Collisions,
		InputType.DistanceFromStart,
		InputType.StartX,
		InputType.StartY,
		InputType.CurrentX,
		InputType.CurrentY,
		InputType.Collided
	};
	
	public ActionType[] EnabledActionNeurons { get; init; } = new[] {
		ActionType.TurnAround,
		ActionType.TurnLeft,
		ActionType.TurnRight,
		ActionType.WalkBackward,
		ActionType.WalkForward,
	};

	public ActivationFunction InternalActivationFunction { get; init; } = ActivationFunction.ReLu;

	public void Deconstruct(out int startNeurons, out int maxInternalNeuron, out ActivationFunction internalActivationFunction) {
		startNeurons = StartNeurons;
		maxInternalNeuron = MaxNeurons;
		internalActivationFunction = InternalActivationFunction;
	}

	public override bool Validate(out ICollection<ValidationResult> results) {
		var isValid = base.Validate(out results);

		if (MaxNeurons < StartNeurons) {
			isValid = false;
			results.Add(new ValidationResult($"{nameof(MaxNeurons)} must be higher then {nameof(StartNeurons)}",
				new[] { nameof(MaxNeurons), nameof(StartNeurons) }));
		}

		if (StartingConnectionsPerNeuron < MinStartingConnectionsPerNeuron) {
			isValid = false;
			results.Add(new ValidationResult($"{nameof(StartingConnectionsPerNeuron)} must be higher then {nameof(MinStartingConnectionsPerNeuron)}",
				new[] { nameof(StartingConnectionsPerNeuron), nameof(MinStartingConnectionsPerNeuron) }));
		}
		
		if (MaxOutgoingConnectionsPerNeuron < StartingConnectionsPerNeuron) {
			isValid = false;
			results.Add(new ValidationResult($"{nameof(MaxOutgoingConnectionsPerNeuron)} must be higher then {nameof(StartingConnectionsPerNeuron)}",
				new[] { nameof(MaxOutgoingConnectionsPerNeuron), nameof(StartingConnectionsPerNeuron) }));
		}

		return isValid;
	}
}
