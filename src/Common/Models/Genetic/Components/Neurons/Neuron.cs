using Common.Helpers;

namespace Common.Models.Genetic.Components.Neurons;

public record Neuron {
	private readonly ushort _id;
	
	private const int TypeMask = (1 << 7);
	private const int IdAndMask = (byte.MaxValue - TypeMask);
	public const int ByteSize = 2;
	private const float BiasDivider = float.MaxValue;
	
	public Neuron(ushort Id, NeuronType NeuronType) {
		this.Id = Id;
		this.NeuronType = NeuronType;
	}
	
	public static float BiasToFloat(float bias) {
		if(bias is < -1f or > 1f) {
			throw new ArgumentOutOfRangeException(nameof(bias), "Bias must be in range [-1, 1]");
		}
		
		return bias * BiasDivider;
	}
	
	public ushort Id {
		get => _id;
		init {
			if(value == 0 || value > (ushort.MaxValue/2)) 
				throw new ArgumentOutOfRangeException(nameof(Id), $"Id must be between 0 and {(ushort.MaxValue/2)+1}");
			
			_id = value;
		}
	}

	public NeuronType NeuronType { get; init; }
	public Neuron ToMemoryNeuron() => this with { NeuronType = NeuronType.Memory };

	public float Bias => 0;

	public void Deconstruct(out ushort Id, out NeuronType NeuronType) {
		Id = this.Id;
		NeuronType = this.NeuronType;
	}

	public string ToHex() => GetBytes().ToHex();

	public byte[] GetBytes() {
		var idBytes = BitConverter.GetBytes(Id).ToList();
		
		// If it's an internal neuron, the leftmost bit is unset
		if(NeuronType != NeuronType.Internal) {
			idBytes[^1] |= TypeMask;
		}

		return idBytes.ToArray();
	}

	public static Neuron FromBytes(byte[] bytes, NeuronType externalType) {
		var type = (bytes[^1] & TypeMask) != 0 ? externalType : NeuronType.Internal;
		bytes[^1] &= IdAndMask;
		var id = BitConverter.ToUInt16(bytes, 0);

		return type switch {
			NeuronType.Input => new InputNeuron(id),
			NeuronType.Action => new ActionNeuron(id),
			NeuronType.Internal => new Neuron(id, NeuronType.Internal),
			_ => throw new ArgumentOutOfRangeException()
		};
		
	}
	
	public static Neuron FromHex(string hex, NeuronType externalType) {	
		var bytes = Convert.FromHexString(hex);
		return FromBytes(bytes, externalType);
	}
}