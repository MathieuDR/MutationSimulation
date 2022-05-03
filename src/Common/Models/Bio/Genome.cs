using Common.Helpers;
using LZStringCSharp;

namespace Common.Models.Bio;

public record Genome :IBiologicalEncodable {
	// NeuronConnections exist out of 2 neurons and a weight in float.
	// Together that are 8 bytes.
	// The hex string is split into 8 byte chunks, which is 16 chars.
	private readonly NeuronConnection[] _neuronConnections;
	private readonly string? _hexSequence;

	public Genome(NeuronConnection[] neuronConnections, string? hexSequence = null) {
		NeuronConnections = neuronConnections;
		HexSequence = hexSequence;
	}

	public NeuronConnection[] NeuronConnections {
		get => _neuronConnections;
		init {
			if (value.Length == 0) {
				throw new ArgumentException("NeuronConnections must not be empty.");
			}

			_neuronConnections = value;
		}
	}

	public string? HexSequence {
		get => _hexSequence ?? ToHex();
		init => _hexSequence = value;
	}


	public byte[] GetBytes() {
		return NeuronConnections.SelectMany(x => x.GetBytes()).ToArray();
	}

	public string ToHex() => LZString.CompressToBase64(GetBytes().ToHex());

	public static Genome FromBytes(byte[] bytes) => new(GenomesFromBytes(bytes));

	private static NeuronConnection[] GenomesFromBytes(byte[] bytes) =>
		bytes.Chunk(NeuronConnection.ByteSize).Select(NeuronConnection.FromBytes).ToArray();

	public static Genome FromHex(string hex) => new(GenomesFromBytes(Convert.FromHexString(LZString.DecompressFromBase64(hex))), hex);

	public void Deconstruct(out NeuronConnection[] Genomes, out string? HexSequence) {
		Genomes = NeuronConnections;
		HexSequence = this.HexSequence;
	}
}
