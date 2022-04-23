using Common.Helpers;

namespace Common.Models; 

public record Genome(Neuron Source, Neuron Destination,float Weight) {
	public override string ToString() => $"{Source}{Destination}{Weight.ToHex()}";
	
	public static Genome FromHex(string hex) {
		// first 2 bytes are source neuron
		var source = Neuron.FromHex(hex.Substring(0, 2), NeuronType.Input);
		
		// next 2 bytes are destination neuron
		var destination = Neuron.FromHex(hex.Substring(2, 2), NeuronType.Output);
		
		// the rest is the weight
		var weight = hex.Substring(4).ToFloat();
		
		return new Genome(source, destination, weight);
	}
}