namespace Common.Models;

public record GenomeSequence {
	public GenomeSequence(Genome[] Genomes, string? HexSequence, string LeftOverSequence) {
		this.Genomes = Genomes;
		this.HexSequence = HexSequence;
		this.LeftOverSequence = LeftOverSequence;
	}

	// Genomes exist out of 2 neurons and a weight in float.
	// Together that are 8 bytes.
	// The hex string is split into 8 byte chunks, which is 16 chars.
	private const int GenomeSequenceLength = 16;
	public Genome[] Genomes { get; init; }
	public string? HexSequence { get; init; }
	public string LeftOverSequence { get; init; }

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
