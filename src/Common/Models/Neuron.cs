using Common.Helpers;

namespace Common.Models;

public record Neuron {
	private readonly ushort _id;
	
	private const int TypeMask = (1 << 7);
	private const int IdAndMask = (byte.MaxValue - TypeMask);
	
	public Neuron(ushort Id, NeuronType NeuronType) {
		this.Id = Id;
		this.NeuronType = NeuronType;
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
	
	public void Deconstruct(out ushort Id, out NeuronType NeuronType) {
		Id = this.Id;
		NeuronType = this.NeuronType;
	}

	public override string ToString() => GetBytes().ToHex();

	public byte[] GetBytes() {
		var idBytes = BitConverter.GetBytes(Id);
		
		// If it's an internal neuron, the leftmost bit is unset
		if(NeuronType != NeuronType.Internal) {
			idBytes[^1] |= TypeMask;
		}
		
		return idBytes;
	}

	public static Neuron FromBytes(byte[] bytes, NeuronType externalType) {
		var type = (bytes[^1] & TypeMask) != 0 ? externalType : NeuronType.Internal;
		bytes[^1] &= IdAndMask;
		var id = BitConverter.ToUInt16(bytes, 0);
		return new Neuron(id, type);
	}
	
	public static Neuron FromHex(string hex, NeuronType externalType) {	
		var bytes = Convert.FromHexString(hex);
		return FromBytes(bytes, externalType);
	}
}
