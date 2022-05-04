namespace Common.Models.Genetic.Components.Neurons; 

public interface INeuron: IBiologicalEncodable, IEquatable<INeuron> {
	public ushort Id { get; init; }
	public NeuronType NeuronType { get; init; }
	public INeuron ToMemoryNeuron();
	
}()
