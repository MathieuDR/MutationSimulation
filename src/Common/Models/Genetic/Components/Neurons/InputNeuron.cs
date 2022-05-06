namespace Common.Models.Genetic.Components.Neurons;

public record InputNeuron : Neuron{
	public InputNeuron(ushort id, InputType inputTypeType): base(id, NeuronType.Input) {
		this.InputTypeType = inputTypeType;
	}
	
	public InputNeuron(ushort id): base(id, NeuronType.Input) {
		var action = id % (Enum.GetValues<InputType>().Length - 1);
		InputTypeType = (InputType)action;
	}
	
	public InputType InputTypeType { get; init; }
	public void Deconstruct(out ushort Id, out NeuronType NeuronType, out InputType inputType) {
		base.Deconstruct(out Id, out NeuronType);
		inputType = this.InputTypeType;
	}
}