namespace Common.Models.Genetic.Components.Neurons;

public record InputNeuron : Neuron{
	protected static int NeuronInputAmount = Enum.GetValues<InputType>().Length;
	public InputNeuron(ushort id, InputType inputType): base(id, NeuronType.Input) {
		this.InputType = inputType;
	}
	
	public InputNeuron(ushort id): base((ushort)(id % NeuronInputAmount), NeuronType.Input) {
		var action = Id;
		InputType = (InputType)action;
	}
	
	public InputType InputType { get; init; }
	public void Deconstruct(out ushort Id, out NeuronType NeuronType, out InputType inputType) {
		base.Deconstruct(out Id, out NeuronType);
		inputType = this.InputType;
	}
}