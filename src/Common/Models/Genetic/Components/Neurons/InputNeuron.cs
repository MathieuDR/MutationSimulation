namespace Common.Models.Genetic.Components.Neurons;

public record InputNeuron : Neuron{
	public static readonly int NeuronInputAmount = Enum.GetValues<InputType>().Length;

	public InputNeuron(ushort id, float bias): base((ushort)(id % NeuronInputAmount), bias, NeuronType.Input) {
		if (Id >= NeuronInputAmount) {
			throw new ArgumentException("Id must be in range of input types", nameof(id));
		}
		
		InputType = (InputType)Id;
	}
	
	public InputType InputType { get; init; }
	public void Deconstruct(out ushort Id, out float Bias, out NeuronType NeuronType, out InputType inputType) {
		base.Deconstruct(out Id, out Bias, out NeuronType);
		inputType = this.InputType;
	}
}