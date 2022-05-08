using Common.Models.Genetic.Components.Neurons;

namespace Common.Helpers; 

public static class ActionTypeHelper {
	public static ActionCategory GetActionCategory(this NeuronAction action) {
		return action switch {
			NeuronAction.WalkForward => ActionCategory.Walking,
			NeuronAction.WalkBackward =>  ActionCategory.Walking,
			NeuronAction.TurnLeft =>  ActionCategory.Turning,
			NeuronAction.TurnRight =>  ActionCategory.Turning,
			NeuronAction.TurnAround =>  ActionCategory.Turning,
			NeuronAction.Eat =>  ActionCategory.Handling,
			NeuronAction.Sleep =>  ActionCategory.Handling,
			NeuronAction.EmitPheromone =>  ActionCategory.Handling,
			NeuronAction.SetOscillator =>  ActionCategory.Internals,
			NeuronAction.SetPheromoneStrength =>  ActionCategory.Internals,
			NeuronAction.SetPheromoneDecay =>  ActionCategory.Internals,
			NeuronAction.SetSpeed =>  ActionCategory.Internals,
			_ => throw new ArgumentOutOfRangeException(nameof(action), action, null)
		};
	}
}
