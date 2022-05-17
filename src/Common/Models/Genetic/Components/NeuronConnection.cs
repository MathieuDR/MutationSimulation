using Common.Helpers;
using Common.Models.Genetic.Components.Neurons;
using QuikGraph;

namespace Common.Models.Genetic.Components; 

public record NeuronConnection : IEdge<Neuron>, IBiologicalEncodable {
	private readonly Neuron _source;
	private readonly Neuron _target;
	private readonly float _weight;
	private const float WeightDivider = float.MaxValue / 4;
	
	public const int ByteSize = Neuron.ByteSize * 2 + sizeof(float) ; 
	
	public static float WeightToFloat(float weight) {
		if(weight is < -4f or > 4f) {
			throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be in range [-4, 4]");
		}
		
		return weight * WeightDivider;
	}
	
	

	public NeuronConnection(Neuron source, Neuron target, float weight) {
		Source = source;
		Target = target;
		Weight = weight;
	}

	public string ToHex() => GetBytes().ToHex();

	public byte[] GetBytes() {
		return Source.GetBytes()
			.Concat(Target.GetBytes())
			.Concat(BitConverter.GetBytes(Weight * WeightDivider))
			.ToArray();
	}
	
	public static NeuronConnection FromBytes(byte[] bytes) {
		// first 2 bytes are source Neuron
		var source = Neuron.FromBytes(bytes.Take(Neuron.ByteSize).ToArray(), NeuronType.Input);
		
		// next 2 bytes are destination Neuron
		var destination = Neuron.FromBytes(bytes.Skip(Neuron.ByteSize).Take(Neuron.ByteSize).ToArray(), NeuronType.Action);
		
		// The rest is the weight and bias
		var weight = BitConverter.ToSingle(bytes.Skip(Neuron.ByteSize * 2).Take(sizeof(float)).ToArray(), 0);
		
		return new NeuronConnection(source, destination, weight);
	} 
	
	public static NeuronConnection FromHex(string hex) {
		var bytes = Convert.FromHexString(hex);
		return FromBytes(bytes);
	}

	public Neuron Source {
		get => _source;
		init {
			if(value.NeuronType == NeuronType.Action) {
				throw new ArgumentException("Source Neuron cannot be an output Neuron");
			}
			_source = value;
		}
	}

	public Neuron Target {
		get => _target;
		init {
			if(value.NeuronType == NeuronType.Input || value.NeuronType == NeuronType.Memory) {
				throw new ArgumentException("Destination Neuron cannot be an input or memory Neuron");
			}
			_target = value;
		}
	}
	public float Weight {
		get => _weight;
		init => _weight = value / WeightDivider;
	}

	public void Deconstruct(out Neuron Source, out Neuron Target, out float Weight) {
		Source = this.Source;
		Target = this.Target;
		Weight = this.Weight;
	}
}