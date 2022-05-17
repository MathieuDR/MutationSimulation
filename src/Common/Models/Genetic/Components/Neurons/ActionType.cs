namespace Common.Models.Genetic.Components.Neurons; 

public enum ActionType {
	WalkForward,
	WalkBackward,
	TurnLeft,
	TurnRight,
	TurnAround,
	Eat,
	Sleep,
	EmitPheromone,
	SetOscillator,
	SetPheromoneStrength,
	SetPheromoneDecay,
	SetSpeed
}