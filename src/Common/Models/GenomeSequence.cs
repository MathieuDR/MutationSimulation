using Common.Helpers;

namespace Common.Models;

public record GenomeSequence {
	private readonly Genome[] _genomes;

	public GenomeSequence(Genome[] Genomes, string? HexSequence = null, string? LeftOverSequence = null) {
		this.Genomes = Genomes;
		this.HexSequence = HexSequence;
		this.LeftOverSequence = LeftOverSequence;
	}

	// Genomes exist out of 2 neurons and a weight in float.
	// Together that are 8 bytes.
	// The hex string is split into 8 byte chunks, which is 16 chars.
	private const int GenomeSequenceLength = 16;

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
	public string? LeftOverSequence { get; init; }

	public byte[] GetBytes() {
		return Genomes.SelectMany(x => x.GetBytes()).ToArray();
	}
	
	public override string ToString() =>  GetBytes().ToHex();

	public static GenomeSequence FromHex(string hex) {
		var genomes = Enumerable.Range(0, hex.Length / GenomeSequenceLength)
			.Select(i => hex.Substring(i * GenomeSequenceLength, GenomeSequenceLength))
			.Select(Genome.FromHex)
			.ToArray();
		
		var leftOver = hex.Substring(genomes.Length * GenomeSequenceLength);
		return new GenomeSequence(genomes, hex, leftOver);
	}

	public void Deconstruct(out Genome[] Genomes, out string? HexSequence, out string LeftOverSequence) {
		Genomes = this.Genomes;
		HexSequence = this.HexSequence;
		LeftOverSequence = this.LeftOverSequence;
	}
};
