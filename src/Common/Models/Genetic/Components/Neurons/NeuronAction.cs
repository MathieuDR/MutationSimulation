namespace Common.Models.Genetic.Components.Neurons; 

public enum NeuronAction {
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