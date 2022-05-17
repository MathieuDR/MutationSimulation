using Common.Models.Genetic.Components.Neurons;

namespace Common.Helpers; 

public static class ActionTypeHelper {
	public static ActionCategory GetActionCategory(this ActionType actionType) {
		return actionType switch {
			ActionType.WalkForward => ActionCategory.Walking,
			ActionType.WalkBackward =>  ActionCategory.Walking,
			ActionType.TurnLeft =>  ActionCategory.Turning,
			ActionType.TurnRight =>  ActionCategory.Turning,
			ActionType.TurnAround =>  ActionCategory.Turning,
			ActionType.Eat =>  ActionCategory.Handling,
			ActionType.Sleep =>  ActionCategory.Handling,
			ActionType.EmitPheromone =>  ActionCategory.Handling,
			ActionType.SetOscillator =>  ActionCategory.Internals,
			ActionType.SetPheromoneStrength =>  ActionCategory.Internals,
			ActionType.SetPheromoneDecay =>  ActionCategory.Internals,
			ActionType.SetSpeed =>  ActionCategory.Internals,
			_ => throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null)
		};
	}
}