using Common.Helpers;

namespace Common.Models;

public record GenomeSequence {
	// Genomes exist out of 2 neurons and a weight in float.
	// Together that are 8 bytes.
	// The hex string is split into 8 byte chunks, which is 16 chars.
	private readonly Genome[] _genomes;

	public GenomeSequence(Genome[] Genomes, string? HexSequence = null) {
		this.Genomes = Genomes;
		this.HexSequence = HexSequence;
	}
	
	public Genome[] Genomes {
		get => _genomes;
		init {
			if (value.Length == 0) {
				throw new ArgumentException("Genomes must not be empty.");
			}
			
			_genomes = value;
		}
	}

	public string? HexSequence { get; init; }

	public byte[] GetBytes() {
		return Genomes.SelectMany(x => x.GetBytes()).ToArray();
	}

	public override string ToString() => GetBytes().ToHex();

	public static GenomeSequence FromBytes(byte[] bytes) => new (GenomesFromBytes(bytes));

	private static Genome[] GenomesFromBytes(byte[] bytes) => bytes.Chunk(Genome.ByteSize).Select(Genome.FromBytes).ToArray();

	public static GenomeSequence FromHex(string hex) => new (GenomesFromBytes(Convert.FromHexString(hex)), hex);

	public void Deconstruct(out Genome[] Genomes, out string? HexSequence) {
		Genomes = this.Genomes;
		HexSequence = this.HexSequence;
	}
};
