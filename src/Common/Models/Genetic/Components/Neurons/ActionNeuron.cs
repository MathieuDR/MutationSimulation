namespace Common.Models.Genetic.Components.Neurons;

public record ActionNeuron : Neuron {
	public static readonly int NeuronActionsAmount = Enum.GetValues<ActionType>().Length;

	 public ActionNeuron(ushort id): base((ushort)(id % NeuronActionsAmount), NeuronType.Action) {
		if (Id >= NeuronActionsAmount) {
			throw new ArgumentException("Id must be in range of action types", nameof(id));
		}
		
		ActionType = (ActionType)Id;
	}
	
	public ActionType ActionType { get; init; }
	
	public void Deconstruct(out ushort Id, out NeuronType NeuronType, out ActionType actionType) {
		base.Deconstruct(out Id, out NeuronType);
		actionType = this.ActionType;
	}
}