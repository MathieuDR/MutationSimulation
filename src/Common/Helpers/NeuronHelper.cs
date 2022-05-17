using Common.Models.Genetic.Components.Neurons;

namespace Common.Helpers;

public static class NeuronHelper {
	public static InputType[] GetEnabledInputTypes() {
		return new[] {
			InputType.Age,
			InputType.Oscillator,
			InputType.LookBack,
			InputType.LookFront,
			InputType.LookLeft,
			InputType.LookRight,
			InputType.Speed
		};
	}
	
	public static ActionType[] GetEnabledActionTypes() {
		return new[] {
			ActionType.TurnAround,
			ActionType.TurnLeft,
			ActionType.TurnRight,
			ActionType.WalkBackward,
			ActionType.WalkForward,
		};
	}

	public static bool IsEnabledNeuron(this Neuron neuron) {
		// type switch
		return neuron switch {
			InputNeuron inputNeuron => inputNeuron.IsEnabledNeuron(),
			ActionNeuron actionNeuron => actionNeuron.IsEnabledNeuron(),
			_ => true
		};
	}

	public static bool IsEnabledNeuron(this ActionNeuron actionNeuron) {
		return GetEnabledActionTypes().Contains(actionNeuron.ActionType);
	}
	
	public static bool IsEnabledNeuron(this InputNeuron inputNeuron) {
		return GetEnabledInputTypes().Contains(inputNeuron.InputType);
	}
}
