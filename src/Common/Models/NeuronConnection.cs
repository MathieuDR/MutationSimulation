using Common.Helpers;

namespace Common.Models; 

public record NeuronConnection {
	private readonly Neuron _source;
	private readonly Neuron _destination;
	private readonly float _weight;
	private const float Divider = float.MaxValue / 4;
	public const int ByteSize = Neuron.ByteSize * 2 + sizeof(float); 
	
	public static float WeightToFloat(float weight) {
		if(weight is < -4f or > 4f) {
			throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be in range [-4, 4]");
		}
		
		return weight * Divider;
	}

	public NeuronConnection(Neuron Source, Neuron Destination,float Weight) {
		this.Source = Source;
		this.Destination = Destination;
		this.Weight = Weight;
	}

	public override string ToString() => GetBytes().ToHex();

	public byte[] GetBytes() {
		return Source.GetBytes()
			.Concat(Destination.GetBytes())
			.Concat(BitConverter.GetBytes(Weight * Divider))
			.ToArray();
	}
	
	public static NeuronConnection FromBytes(byte[] bytes) {
		// first 2 bytes are source neuron
		var source = Neuron.FromBytes(bytes.Take(Neuron.ByteSize).ToArray(), NeuronType.Input);
		
		// next 2 bytes are destination neuron
		var destination = Neuron.FromBytes(bytes.Skip(Neuron.ByteSize).Take(Neuron.ByteSize).ToArray(), NeuronType.Output);
		
		// The rest is the weight
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
			if(value.NeuronType == NeuronType.Output) {
				throw new ArgumentException("Source neuron cannot be an output neuron");
			}
			_source = value;
		}
	}

	public Neuron Destination {
		get => _destination;
		init {
			if(value.NeuronType == NeuronType.Input) {
				throw new ArgumentException("Destination neuron cannot be an input neuron");
			}
			_destination = value;
		}
	}
	
	public float Weight {
		get => _weight;
		init => _weight = value / Divider;
	}

	public void Deconstruct(out Neuron Source, out Neuron Destination, out float Weight) {
		Source = this.Source;
		Destination = this.Destination;
		Weight = this.Weight;
	}
}