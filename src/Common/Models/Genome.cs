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
	public NeuronConnection[][] LayeredConnections { get; private set; }
	
	public Neuron[] ChainedNeurons { get; private set; }
	
	public NeuronConnection[] NeuronConnections {
		get => _neuronConnections;
		init {
			if (value.Length == 0) {
				throw new ArgumentException("NeuronConnections must not be empty.");
			}
			
			_neuronConnections = value;
			UsedConnections = GetUsedConnections(value);
			LayeredConnections = LayerConnections();
			ChainedNeurons = GetChainedNeurons();
		}
	}

	private Neuron[] GetChainedNeurons() {
		var neurons = new List<Neuron>();
		
		foreach (var layeredConnection in LayeredConnections) {
			var destinations = new List<Neuron>();
			
			foreach (var connection in layeredConnection) {
				neurons.Add(connection.Source);
				destinations.Add(connection.Destination);
			}
			neurons.AddRange(destinations);
		}

		return neurons.Distinct().ToArray();
	}

	private NeuronConnection[][] LayerConnections(int count = 0) {
		if (count > 10) {
			throw new ArgumentException("Too many recursions. something is up..");
		}
		
		var layers = new List<IEnumerable<NeuronConnection>>();

		var connectionPool = UsedConnections.ToList();
		var layer = connectionPool.ToList();
		var loopCount = 0;
		var failed = false;

		do {
			// var destinations = layer.Select(x => x.Destination).Distinct();
			
			loopCount++;
			if(loopCount > 100) {
				throw new Exception($"Genome: {ToHex()} has too many loops.");
			}

			var pruned = new List<NeuronConnection>();
			var dict = new Dictionary<NeuronConnection, List<NeuronConnection>>();
			foreach (var connection in layer) {
				if (connection.Source.NeuronType != NeuronType.Input) {
					var connections = connectionPool.Where(x => x.Destination == connection.Source && x != connection).ToList();

					if (connections.Any()) {
						dict.Add(connection, connections);
						continue;
					}
				}

				pruned.Add(connection);
				connectionPool.Remove(connection);
			}

			if (!pruned.Any()) {
				if (connectionPool.Any()) {
					// remove and fix dependency loop.
					
					// first check if any of the depency loops are from a connection that have no other 'ins' then the current node
					// if so, remove those and retry
					foreach (var kvp in dict) {
						var conns = UsedConnections.Where(x=> x )
					}
					
					
					// if not, remove the one with the lowest count
					// select the key with the lowest count
					var key = dict.MinBy(x => x.Value.Count).Key;
					// remove connections from connection pool
					var newUsedConn = UsedConnections.ToList();
					newUsedConn.RemoveAll(x => dict[key].Contains(x) && x.Destination.NeuronType != NeuronType.Output);
					UsedConnections = GetUsedConnections(newUsedConn);

					failed = true;
				}

				// requeue
				break;
			}

			layers.Add(pruned);
			layer = connectionPool.Where(x => pruned.Select(y=>y.Destination).Contains(x.Source)).ToList();
		} while (layer.Any());

		if (failed) {
			return LayerConnections(count+1);
		}

		return layers.Select(x => x.ToArray()).ToArray();
	}

	private NeuronConnection[] GetUsedConnections(IEnumerable<NeuronConnection> connections) {
		var usedConnections = new List<NeuronConnection>();
		var unusedConnections = new List<NeuronConnection>(connections);

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

	public string ToHex() => GetBytes().ToHex();

	public static Genome FromBytes(byte[] bytes) => new (GenomesFromBytes(bytes));

	private static NeuronConnection[] GenomesFromBytes(byte[] bytes) => bytes.Chunk(NeuronConnection.ByteSize).Select(NeuronConnection.FromBytes).ToArray();

	public static Genome FromHex(string hex) => new (GenomesFromBytes(Convert.FromHexString(hex)), hex);

	public void Deconstruct(out NeuronConnection[] Genomes, out string? HexSequence) {
		Genomes = this.NeuronConnections;
		HexSequence = this.HexSequence;
	}
}

public record NeuronConnectionTree(NeuronConnection Item, IEnumerable<NeuronConnectionTree> Children);

