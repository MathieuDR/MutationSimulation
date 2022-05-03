namespace Common.Models.Genetic.Components.Neurons;

public interface IInputNeuron : INeuron {
	public NeuronInput NeuronInput { get; init; }
}