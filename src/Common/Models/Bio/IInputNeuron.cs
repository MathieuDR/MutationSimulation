namespace Common.Models.Bio;

public interface IInputNeuron : INeuron {
	public NeuronInput NeuronInput { get; init; }
}