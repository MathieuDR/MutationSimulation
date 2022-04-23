using Common.Helpers;

namespace Common.Models;

public record Neuron {
	private readonly byte _id;
	
	private const int IdBitSize = sizeof(byte) * 8;
	private const int IdLeftMostBit = IdBitSize-1;
	private const int TypeMask = (1 << IdLeftMostBit);
	private const int IdAndMask = (byte.MaxValue - TypeMask);
	
	public Neuron(byte Id, NeuronType NeuronType) {
		this.Id = Id;
		this.NeuronType = NeuronType;
	}
	public override string ToString() => this.ToHex();

	public byte ToByte() {
		
		// If it's an internal neuron, the leftmost bit is unset
		if(NeuronType != NeuronType.Internal) {
			return (byte)(Id | TypeMask);
		}
		
		return Id;
	}
	
	
	public static Neuron FromByte(byte @byte, NeuronType externalType) {
		var type = (@byte & TypeMask) != 0 ? externalType : NeuronType.Internal;
		var id = (byte)(@byte & IdAndMask);
		return new Neuron(id, type);
	}

	public byte Id {
		get => _id;
		init {
			if(value == 0 || value > (byte.MaxValue/2)) 
				throw new ArgumentOutOfRangeException(nameof(Id), $"Id must be between 0 and {(byte.MaxValue/2)+1}");
			
			_id = value;
		}
	}

	public NeuronType NeuronType { get; init; }
	public void Deconstruct(out byte Id, out NeuronType NeuronType) {
		Id = this.Id;
		NeuronType = this.NeuronType;
	}
}
