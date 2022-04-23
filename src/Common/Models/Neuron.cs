using Common.Helpers;

namespace Common.Models;

public record Neuron {
	private readonly byte _id;

	public Neuron(byte Id, NeuronType NeuronType) {
		this.Id = Id;
		this.NeuronType = NeuronType;
	}
	public override string ToString() => this.ToHex();

	public byte ToByte() {
		// If it's an internal neuron, the leftmost bit is unset
		if(NeuronType != NeuronType.Internal) {
			return (byte)(Id | 0b1000_0000);
		}
		
		return Id;
	}
	
	
	public static Neuron FromByte(byte @byte, NeuronType externalType) {
		var type = (@byte & 0b1000_0000) != 0 ? externalType : NeuronType.Internal;
		var id = (byte)(@byte & 0b0111_1111);
		return new Neuron(id, type);
	}

	public byte Id {
		get => _id;
		init {
			if(value == 0 || value >= 128) 
				throw new ArgumentOutOfRangeException(nameof(Id), "Id must be between 0 and 128");
			
			_id = value;
		}
	}

	public NeuronType NeuronType { get; init; }
	public void Deconstruct(out byte Id, out NeuronType NeuronType) {
		Id = this.Id;
		NeuronType = this.NeuronType;
	}
}
