namespace Common.Models.Genetic.Components.Neurons;

public record ActionNeuron : Neuron {
	public ActionNeuron(ushort id, NeuronAction neuronAction): base(id, NeuronType.Action) {
		NeuronAction = neuronAction;
	}
	
	public ActionNeuron(ushort id): base(id, NeuronType.Action) {
		var action = id % (Enum.GetValues<NeuronAction>().Length - 1);
		NeuronAction = (NeuronAction)action;
	}
	
	public NeuronAction NeuronAction { get; init; }
	
	public void Deconstruct(out ushort Id, out NeuronType NeuronType, out NeuronAction NeuronAction) {
		base.Deconstruct(out Id, out NeuronType);
		NeuronAction = this.NeuronAction;
	}
}