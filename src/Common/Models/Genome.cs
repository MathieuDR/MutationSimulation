using Common.Helpers;

namespace Common.Models;

public record Genome {
	// NeuronConnections exist out of 2 neurons and a weight in float.
	// Together that are 8 bytes.
	// The hex string is split into 8 byte chunks, which is 16 chars.
	private readonly NeuronConnection[] _neuronConnections;

	public Genome(NeuronConnection[] neuronConnections, string? hexSequence = null) {
		NeuronConnections = neuronConnections;
		HexSequence = hexSequence;
	}

	public NeuronConnection[] UsedConnections { get; private set; }
	
	public NeuronConnection[] NeuronConnections {
		get => _neuronConnections;
		init {
			if (value.Length == 0) {
				throw new ArgumentException("NeuronConnections must not be empty.");
			}
			
			_neuronConnections = value;
			UsedConnections = GetUsedConnections();
		}
	}

	private NeuronConnection[] GetUsedConnections() {
		var usedConnections = new List<NeuronConnection>();
		var unusedConnections = new List<NeuronConnection>(NeuronConnections);

		PruneConnections(usedConnections, unusedConnections);
		return usedConnections.ToArray();
	}

	private void PruneConnections(List<NeuronConnection> usedConnections, List<NeuronConnection> unusedConnections, NeuronConnection? current = default) {
		var connections = current == default ? 
			unusedConnections.Where(c => c.Destination.NeuronType == NeuronType.Output).ToArray() : 
			unusedConnections.Where(c => c.Destination == current.Source).ToArray();

		if (!connections.Any()) {
			return;
		}

		usedConnections.AddRange(connections);
		unusedConnections.RemoveAll(c => connections.Contains(c));
		
		if(unusedConnections.Count == 0) {
			return;
		}
		
		foreach(var connection in connections) {
			PruneConnections(usedConnections, unusedConnections, connection);
		}
	}
	
	public IEnumerable<NeuronConnectionTree> GenerateTree(NeuronConnection currentNeuronConnection) {
		foreach (var childGenome in NeuronConnections.Where(c => c.Source.Id == currentNeuronConnection.Destination.Id && currentNeuronConnection != c)) {
			yield return new NeuronConnectionTree(childGenome, GenerateTree(childGenome));
		}
	}
	
	// reverse tree
	

	public string? HexSequence { get; init; }

	public byte[] GetBytes() {
		return NeuronConnections.SelectMany(x => x.GetBytes()).ToArray();
	}

	public override string ToString() => GetBytes().ToHex();

	public static Genome FromBytes(byte[] bytes) => new (GenomesFromBytes(bytes));

	private static NeuronConnection[] GenomesFromBytes(byte[] bytes) => bytes.Chunk(NeuronConnection.ByteSize).Select(NeuronConnection.FromBytes).ToArray();

	public static Genome FromHex(string hex) => new (GenomesFromBytes(Convert.FromHexString(hex)), hex);

	public void Deconstruct(out NeuronConnection[] Genomes, out string? HexSequence) {
		Genomes = this.NeuronConnections;
		HexSequence = this.HexSequence;
	}
}

public record NeuronConnectionTree(NeuronConnection Item, IEnumerable<NeuronConnectionTree> Children);

