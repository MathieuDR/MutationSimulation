using Common.Helpers;

namespace Common.Models; 

public record Genome(Neuron Source, Neuron Destination,float Weight) {
	public override string ToString() => $"{Source}{Destination}{Weight.ToHex()}";
}

public record Neuron();