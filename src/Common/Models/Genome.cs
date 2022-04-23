using Common.Helpers;

namespace Common.Models; 

public record Genome {
	private readonly Neuron _source;
	private readonly Neuron _destination;
	private readonly float _weight;
	private const float Divider = float.MaxValue / 4;

	public Genome(Neuron Source, Neuron Destination,float Weight) {
		this.Source = Source;
		this.Destination = Destination;
		this.Weight = Weight;
	}

	public override string ToString() => BitConverter.ToInt64( GetBytes(),0).ToHex();

	public byte[] GetBytes() {
		return
			BitConverter.GetBytes(Weight*Divider)
			.Concat(Destination.GetBytes())
			.Concat(Source.GetBytes())
			.ToArray();
	}
	
	public static Genome FromHex(string hex) {
		// first 2 bytes are source neuron
		var source = Neuron.FromHex(hex.Substring(0, 4), NeuronType.Input);
		
		// next 2 bytes are destination neuron
		var destination = Neuron.FromHex(hex.Substring(4, 4), NeuronType.Output);
		
		// the rest is the weight
		var weight = hex.Substring(8).ToFloat();
		
		return new Genome(source, destination, weight);
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