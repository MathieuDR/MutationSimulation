using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ConnectedComponents;
using QuikGraph.Algorithms.Search;

namespace Common.Models.Genetic;

public record Brain {
	private readonly Genome _genome;

	public Brain(Genome genome) => Genome = genome;

	public NeuronConnection[] SortedConnections { get; private set; }
	public NeuronConnection[] MemoryConnections { get; private set; }
	public INeuron[] SortedNeurons { get; private set; }
	public AdjacencyGraph<INeuron, NeuronConnection> BrainGraph { get; private set; }

	public Genome Genome {
		get => _genome;
		init {
			CreateGraph(value.NeuronConnections);
			_genome = value;
		}
	}

	private IEnumerable<NeuronConnection> CalculatedUsedConnections(IEnumerable<NeuronConnection> connections) {
		var neuronConnections = connections as NeuronConnection[] ?? connections.ToArray();
		
		var reversedGraph = neuronConnections
			.Select(x => new Edge<INeuron>(x.Target, x.Source))
			.ToAdjacencyGraph<INeuron, Edge<INeuron>>(false);


		var connectedVertices = new List<INeuron>();
		var alg = new BreadthFirstSearchAlgorithm<INeuron, Edge<INeuron>>(reversedGraph);
		alg.DiscoverVertex += vertex => connectedVertices.Add(vertex);

		foreach (var vertex in reversedGraph.Vertices.Where(x => x.NeuronType == NeuronType.Action)) {
			alg.Compute(vertex);
		}

		return neuronConnections.Where(x => connectedVertices.Contains(x.Source) && connectedVertices.Contains(x.Target));
	}

	private void CreateGraph(NeuronConnection[] connections) {
		var usedConnections = CalculatedUsedConnections(connections).ToArray();
		var selfReferences = usedConnections.Where(x => x.Source == x.Target).Select(ToMemoryConnection).ToArray();
		var withMemory = usedConnections.Where(x => x.Source != x.Target).Concat(selfReferences).ToArray();

		BrainGraph = withMemory.ToAdjacencyGraph<INeuron, NeuronConnection>(false);
		EnsureAcyclicGraph();

		// sort
		SortedNeurons = BrainGraph.TopologicalSort().ToArray();

		// fix the connections
		SortConnections();
	}

	private NeuronConnection ToMemoryConnection(NeuronConnection connection) => new NeuronConnection(
		connection.Source.ToMemoryNeuron(), connection.Target, NeuronConnection.WeightToFloat(connection.Weight));

	private void SortConnections() {
		var sortedConnections = new List<NeuronConnection>();
		foreach (var neuron in SortedNeurons) {
			var outEdges = BrainGraph.OutEdges(neuron);
			sortedConnections.AddRange(outEdges);
		}

		SortedConnections = sortedConnections.ToArray();
	}

	private void EnsureAcyclicGraph() {
		var counter = 0;
		List<BidirectionalGraph<INeuron, NeuronConnection>> components;
		var componentsCountDict = new Dictionary<INeuron, int>();
		var cCAlg = new StronglyConnectedComponentsAlgorithm<INeuron, NeuronConnection>(BrainGraph, componentsCountDict);

		var memoryConnection = new List<NeuronConnection>();

		do {
			cCAlg.Compute();
			components = cCAlg.Graphs.Where(x => x.EdgeCount > 0 && x.VertexCount > 1).ToList();

			if (!components.Any()) {
				continue;
			}

			var edgeWithUsed = FindBestMemoryCandidates(cCAlg);
			MakeCandidatesToMemories(edgeWithUsed, memoryConnection);
		} while (counter++ < 10 && components.Any());

		MemoryConnections = memoryConnection.ToArray();
	}

	private void MakeCandidatesToMemories(Dictionary<INeuron, (NeuronConnection connection, int removedConnections)> edgeWithUsed,
		List<NeuronConnection> memoryConnection) {
		foreach (var kvp in edgeWithUsed) {
			var memory = ToMemoryConnection(kvp.Value.connection);
			BrainGraph.RemoveEdge(kvp.Value.connection);
			memoryConnection.Add(memory);
			BrainGraph.AddVerticesAndEdge(memory);
		}
	}

	private Dictionary<INeuron, (NeuronConnection connection, int removedConnections)> FindBestMemoryCandidates(
		StronglyConnectedComponentsAlgorithm<INeuron, NeuronConnection> cCAlg) {
		var neuronsToCheck = FindCyclicNeurons(cCAlg);

		// var componentDone = new List<INeuron>();
		var edgeWithUsed = new Dictionary<INeuron, (NeuronConnection connection, int removedConnections)>();
		foreach (var kvp in neuronsToCheck) {
			if (edgeWithUsed.Any(x => x.Key == kvp.Value && x.Value.removedConnections == 1)) {
				continue;
			}

			if (TryFindEdgeByStronglyConnectedComponents(cCAlg, kvp, out var outEdge)) {
				continue;
			}

			var removed = CalculateUsedEdgesOnRemoval(outEdge);
			SaveToDictionary(edgeWithUsed, kvp, removed, outEdge);
		}

		return edgeWithUsed;
	}

	private bool TryFindEdgeByStronglyConnectedComponents(StronglyConnectedComponentsAlgorithm<INeuron, NeuronConnection> cCAlg,
		KeyValuePair<INeuron, INeuron> kvp, out NeuronConnection? outEdge) {
		outEdge = BrainGraph
			.OutEdges(kvp.Key)
			.FirstOrDefault(x => x.Target == kvp.Value || (cCAlg.Roots.ContainsKey(x.Target) && cCAlg.Roots[x.Target] == kvp.Value));

		return outEdge is null;
	}

	private static void SaveToDictionary(Dictionary<INeuron, (NeuronConnection connection, int removedConnections)> edgeWithUsed,
		KeyValuePair<INeuron, INeuron> kvp, int removed, NeuronConnection outEdge) {
		if (edgeWithUsed.TryGetValue(kvp.Value, out var tuple)) {
			if (tuple.removedConnections > removed) {
				edgeWithUsed[kvp.Value] = (outEdge, removed);
			}

			return;
		}

		edgeWithUsed.Add(kvp.Value, (outEdge, removed));
	}

	private int CalculateUsedEdgesOnRemoval(NeuronConnection outEdge) {
		var tempEdges = BrainGraph.Edges.Where(x => x != outEdge);
		var used = CalculatedUsedConnections(tempEdges).Count();
		var removed = BrainGraph.Edges.Count() - used;
		return removed;
	}

	private static IEnumerable<KeyValuePair<INeuron, INeuron>> FindCyclicNeurons(StronglyConnectedComponentsAlgorithm<INeuron, NeuronConnection> cCAlg) {
		var componentGroups = cCAlg.Components
			.GroupBy(x => x.Value).Where(x => x.Count() > 1)
			.SelectMany(x => x);

		var neuronsToCheck = cCAlg.Roots.Where(x => componentGroups.Any(kvp => kvp.Key == x.Key));
		return neuronsToCheck;
	}

	public void Deconstruct(out Genome Genome) {
		Genome = this.Genome;
	}
}
