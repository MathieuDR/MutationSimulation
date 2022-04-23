using BenchmarkDotNet.Attributes;
using Common.Helpers;
using Common.Models;

namespace Main;

[MemoryDiagnoser(false)]
public class BenchMarker {
	public Neuron Neuron => new Neuron(4, NeuronType.Internal);
	
	[Benchmark]
	public string HexFromConverter() {
		var bytes = Neuron
			.GetBytes()
			.Reverse()
			.ToArray();
		
		return Convert.ToHexString(bytes);
	}
	
	[Benchmark]
	public string HexFromToString() {
		var bytes = Neuron
			.GetBytes();
		
		ushort @short = BitConverter.ToUInt16(bytes, 0);
		return @short.ToHex();
	}
	
	[Benchmark]
	public void GetBytes() {
		var bytes = Neuron
			.GetBytes();
	}
	
	[Benchmark]
	public void GetReversedBytes() {
		var bytes = Neuron
			.GetBytes()
			.Reverse()
			.ToArray();
	}
	
}
