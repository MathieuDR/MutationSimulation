using System.Diagnostics;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Algorithms.ConnectedComponents;
using QuikGraph.Algorithms.Search;

namespace Common.Models;

public record Brain {
	private readonly Genome _genome;

	public Brain(Genome genome) {
		this.Genome = genome;
	}
	public NeuronConnection[] SortedConnections { get; private set; }
	public NeuronConnection[] MemoryConnections { get; private set; }
	public Neuron[] SortedNeurons { get; private set; }
	public AdjacencyGraph<Neuron,NeuronConnection> BrainGraph { get; private set; }
	

	public Genome Genome {
		get => _genome;
		init {
			CreateGraph(value.NeuronConnections);
			_genome = value;
		}
	}

	private IEnumerable<NeuronConnection> CalculatedUsedConnections(IEnumerable<NeuronConnection> connections) {
		var neuronConnections = connections as NeuronConnection[] ?? connections.ToArray();
		var reversedGraph = neuronConnections.Select(x => new Edge<Neuron>(x.Target, x.Source))
			.ToAdjacencyGraph<Neuron, Edge<Neuron>>(false);


		var connectedVertices = new List<Neuron>();
		var alg = new BreadthFirstSearchAlgorithm<Neuron, Edge<Neuron>>(reversedGraph);
		alg.DiscoverVertex += vertex => connectedVertices.Add(vertex);

		foreach (var vertex in reversedGraph.Vertices.Where(x => x.NeuronType == NeuronType.Output)) {
			alg.Compute(vertex);
		}

		return neuronConnections.Where(x => connectedVertices.Contains(x.Source) && connectedVertices.Contains(x.Target));
	}

	private void CreateGraph(NeuronConnection[] connections) {
		var cons = CalculatedUsedConnections(connections).ToArray();
		var selfReferences = cons.Where(x => x.Source == x.Target).Select(ToMemoryConnection).ToArray();
		var withMemory = cons.Where(x => x.Source != x.Target).Concat(selfReferences).ToArray();
		
		BrainGraph = withMemory.ToAdjacencyGraph<Neuron, NeuronConnection>(false);
		EnsureAcyclicGraph(cons);

		// sort
		SortedNeurons = BrainGraph.TopologicalSort().ToArray();
		
		// fix the connections
		SortConnections();
	}
	
	private NeuronConnection ToMemoryConnection(NeuronConnection connection) {
		return new NeuronConnection(connection.Source with { NeuronType = NeuronType.Memory }, connection.Target, NeuronConnection.WeightToFloat(connection.Weight));
	}

	private void SortConnections() {
		var sortedConnections = new List<NeuronConnection>();
		foreach (var neuron in SortedNeurons) {
			var outEdges = BrainGraph.OutEdges(neuron);
			
			// add self refences before the others
			//var selfRef = selfReferences.FirstOrDefault(x => x.Source == neuron);
			// if (selfRef is not null) {
			// 	sortedConnections.Add(selfRef);
			// }

			sortedConnections.AddRange(outEdges);
		}

		// foreach (var selfReference in selfReferences) {
		// 	BrainGraph.AddEdge(selfReference);
		// }
		
		SortedConnections = sortedConnections.ToArray();
	}

	private void EnsureAcyclicGraph(NeuronConnection[] cons) {
		var counter = 0;
		List<BidirectionalGraph<Neuron, NeuronConnection>> components;
		var componentsCountDict = new Dictionary<Neuron, int>();
		var cCAlg = new StronglyConnectedComponentsAlgorithm<Neuron, NeuronConnection>(BrainGraph, componentsCountDict);

		var memoryConnection = new List<NeuronConnection>();

		do {
			cCAlg.Compute();
			components = cCAlg.Graphs.Where(x => x.EdgeCount > 0 && x.VertexCount > 1).ToList();

			if (!components.Any()) {
				continue;
			}

			var componentGroups = cCAlg.Components
					.GroupBy(x => x.Value).Where(x => x.Count() > 1)
					.SelectMany(x => x);
			
			var neuronsToCheck = cCAlg.Roots.Where(x => componentGroups.Any(kvp => kvp.Key == x.Key));
			
			
			
			// var componentDone = new List<Neuron>();
			var edgeWithUsed = new Dictionary<Neuron, (NeuronConnection connection, int removedConnections)>();
			foreach (var kvp in neuronsToCheck) {
				if(edgeWithUsed.Any(x=> x.Key == kvp.Value && x.Value.removedConnections == 1)) {
					continue;
				}
				
				var outEdge = BrainGraph.OutEdges(kvp.Key).FirstOrDefault(x=>x.Target == kvp.Value || (cCAlg.Roots.ContainsKey(x.Target) && cCAlg.Roots[x.Target] == kvp.Value));
				if (outEdge is null) {
					continue;
				}
				
				var tempEdges = BrainGraph.Edges.Where(x => x != outEdge);
				var used = CalculatedUsedConnections(tempEdges).Count();
				var removed = BrainGraph.Edges.Count() - used;

				if (edgeWithUsed.TryGetValue(kvp.Value, out var tuple)) {
					if (tuple.removedConnections > removed) {
						edgeWithUsed[kvp.Value] = (outEdge, removed);
					}
					
					continue;
				}
				
				edgeWithUsed.Add(kvp.Value, (outEdge, removed));
			}

			foreach (var kvp in edgeWithUsed) {
				var memory = ToMemoryConnection(kvp.Value.connection);
				BrainGraph.RemoveEdge(kvp.Value.connection);
				memoryConnection.Add(memory);
				BrainGraph.AddVerticesAndEdge(memory);
			}
		} while (counter++ < 10 && components.Any());

		MemoryConnections = memoryConnection.ToArray();
	}

	public void Deconstruct(out Genome Genome) {
		Genome = this.Genome;
	}
};
