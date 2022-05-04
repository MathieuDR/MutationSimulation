namespace Common.Models.Genetic.Components.Neurons;

public record InputNeuron : Neuron{
	public InputNeuron(ushort id, NeuronInput neuronInput): base(id, NeuronType.Input) {
		this.NeuronInput = neuronInput;
	}
	
	public InputNeuron(ushort id): base(id, NeuronType.Input) {
		var action = id % (Enum.GetValues<NeuronInput>().Length - 1);
		NeuronInput = (NeuronInput)action;
	}
	
	public NeuronInput NeuronInput { get; init; }
	public void Deconstruct(out ushort Id, out NeuronType NeuronType, out NeuronInput NeuronInput) {
		base.Deconstruct(out Id, out NeuronType);
		NeuronInput = this.NeuronInput;
	}
}