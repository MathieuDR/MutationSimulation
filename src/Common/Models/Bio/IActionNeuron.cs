namespace Common.Models.Bio;

public interface IActionNeuron : INeuron {
	public NeuronAction NeuronAction { get; init; }
}