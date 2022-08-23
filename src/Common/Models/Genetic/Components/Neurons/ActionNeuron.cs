namespace Common.Models.Genetic.Components.Neurons;

public record ActionNeuron : Neuron {
	public static readonly int NeuronActionsAmount = Enum.GetValues<ActionType>().Length;

	 public ActionNeuron(ushort id, float bias): base((ushort)(id % NeuronActionsAmount), bias, NeuronType.Action) {
		if (Id >= NeuronActionsAmount) {
			throw new ArgumentException("Id must be in range of action types", nameof(id));
		}
		
		ActionType = (ActionType)Id;
	}
	
	public ActionType ActionType { get; init; }
	
	public void Deconstruct(out ushort Id, out float Bias, out NeuronType NeuronType, out ActionType actionType) {
		base.Deconstruct(out Id, out Bias, out NeuronType);
		actionType = this.ActionType;
	}
}