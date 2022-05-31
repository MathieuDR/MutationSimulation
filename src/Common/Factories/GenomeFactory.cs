using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using Common.Models.Options;
using Common.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Factories;

public class GenomeFactory {
	private readonly BrainOptions _brainOptions;
	private readonly ILogger<GenomeFactory> _logger;
	private readonly Random _random;

	public GenomeFactory(ILogger<GenomeFactory> logger, IOptionsSnapshot<BrainOptions> brainOptions, IRandomProvider randomProvider) {
		_logger = logger;
		_brainOptions = brainOptions.Value;
		_random = randomProvider.GetRandom();
	}

	public IEnumerable<Genome> Create(int count) {
		for (var i = 0; i < count; i++) {
			var neuronsAmount = _brainOptions.MinStartNeurons.HasValue
				? _random.Next(_brainOptions.MinStartNeurons.Value, _brainOptions.StartNeurons + 1)
				: _brainOptions.StartNeurons;

			var neurons = CreateNeurons(neuronsAmount).ToArray();
			var connections = CreateConnections(neurons).ToArray();
			yield return new Genome(connections);
		}
	}


	private IEnumerable<NeuronConnection> CreateConnections(IEnumerable<Neuron> neurons) {
		var connections = new List<NeuronConnection>();
		var neuronArray = neurons.ToArray();
		var withoutInputs = neuronArray.Where(x => x.NeuronType != NeuronType.Input).ToArray();

		foreach (var neuron in neuronArray) {
			if (neuron.NeuronType == NeuronType.Action) {
				continue;
			}

			var toChooseFrom = withoutInputs.ToList();

			var connectionsAmount = _brainOptions.MinStartingConnectionsPerNeuron.HasValue
				? _random.Next(_brainOptions.MinStartingConnectionsPerNeuron.Value, _brainOptions.StartingConnectionsPerNeuron + 1)
				: _brainOptions.StartingConnectionsPerNeuron;

			connectionsAmount = Math.Min(toChooseFrom.Count, connectionsAmount);

			var conns = 0;
			while (conns++ < connectionsAmount) {
				var n2 = NextFromList(_random, toChooseFrom);
				toChooseFrom.Remove(n2);

				var weight = NextWeight(_random);
				connections.Add(new NeuronConnection(neuron, n2, weight));
			}
		}

		return connections;
	}

	private static T NextFromList<T>(Random random, List<T> list) {
		// get random from list
		var index = random.Next(0, list.Count);
		return list[index];
	}

	private static float NextWeight(Random random) {
		// number between -4 and 4 
		var f = (float)random.NextDouble() * 8 - 4;
		return NeuronConnection.WeightToFloat(f);
	}

	private IEnumerable<Neuron> CreateNeurons(int count) {
		if (!_brainOptions.EnabledInputNeurons.Any()) {
			throw new InvalidOperationException("No valid input neurons");
		}

		if (!_brainOptions.EnabledActionNeurons.Any()) {
			throw new InvalidOperationException("No valid action neurons");
		}

		for (var i = 0; i < count; i++) {
			var id = (ushort)_random.Next(0, _brainOptions.MaxNeurons);
			var randNum = _random.NextDouble();
			var isInternal = randNum < _brainOptions.InternalRate;

			if (isInternal) {
				yield return new Neuron(id, NeuronType.Internal);
				continue;
			}

			yield return GetIONeuron(id);
		}
	}

	private Neuron GetIONeuron(ushort id) {
		var randNum = _random.NextDouble();
		var nonInternalType = randNum < _brainOptions.InputRate ? NeuronType.Input : NeuronType.Action;
		var mod = nonInternalType switch {
			NeuronType.Input => InputNeuron.NeuronInputAmount,
			NeuronType.Action => ActionNeuron.NeuronActionsAmount,
			_ => throw new ArgumentOutOfRangeException(nameof(nonInternalType))
		};


		while (!_brainOptions.EnabledInputNeurons.Contains((InputType)(id % mod))) {
			id++;
		}

		id = (ushort)(id % mod);

		return nonInternalType switch {
			NeuronType.Input => new InputNeuron(id),
			NeuronType.Action => new ActionNeuron(id),
			_ => throw new ArgumentOutOfRangeException(nameof(nonInternalType))
		};
	}
}
