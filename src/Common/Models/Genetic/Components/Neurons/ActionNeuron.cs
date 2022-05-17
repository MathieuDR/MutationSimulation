namespace Common.Models.Genetic.Components.Neurons;

public record ActionNeuron : Neuron {
	protected static int NeuronActionsAmount = Enum.GetValues<ActionType>().Length;
	public ActionNeuron(ushort id, ActionType actionType): base(id, NeuronType.Action) {
		ActionType = actionType;
	}
	
	public ActionNeuron(ushort id): base((ushort)(id % NeuronActionsAmount), NeuronType.Action) {
		var action = Id; 
		ActionType = (ActionType)action;
	}
	
	public ActionType ActionType { get; init; }
	
	public void Deconstruct(out ushort Id, out NeuronType NeuronType, out ActionType actionType) {
		base.Deconstruct(out Id, out NeuronType);
		actionType = this.ActionType;
	}
}