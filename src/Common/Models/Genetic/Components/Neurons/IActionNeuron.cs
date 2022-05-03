namespace Common.Models.Genetic.Components.Neurons;

public interface IActionNeuron : INeuron {
	public NeuronAction NeuronAction { get; init; }
}