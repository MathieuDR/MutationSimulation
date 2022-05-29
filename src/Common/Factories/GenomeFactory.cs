using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using Common.Models.Options;
using Common.Simulator;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Factories;

internal static class GenomeFactory {
	public static IEnumerable<Genome> CreateGenomes(IServiceProvider serviceProvider, int count) {
		var brainOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<BrainOptions>>().Value;
		var random = RandomProvider.GetRandom();

		for (var i = 0; i < count; i++) {
			var neuronsAmount = brainOptions.MinStartNeurons.HasValue
				? random.Next(brainOptions.MinStartNeurons.Value, brainOptions.StartNeurons + 1)
				: brainOptions.StartNeurons;

			var neurons = CreateNeurons(neuronsAmount, brainOptions, random);
			var connections = CreateConnections(neurons, brainOptions, random);

			yield return new Genome(connections.ToArray());
		}
	}


	private static IEnumerable<NeuronConnection> CreateConnections(IEnumerable<Neuron> neurons, BrainOptions options, Random random) {
		var connections = new List<NeuronConnection>();
		var neuronArray = neurons as Neuron[] ?? neurons.ToArray();
		//var withoutActions = neuronArray.Where(x => x.NeuronType != NeuronType.Action);
		var withoutInputs = neuronArray.Where(x => x.NeuronType != NeuronType.Input).ToArray();
		
		foreach (var neuron in neuronArray) {
			if(neuron.NeuronType == NeuronType.Action)
				continue;
			
			// does this make a deepcopy. Test this!
			var toChooseFrom = withoutInputs.ToList();

			var connectionsAmount = options.MinStartingConnectionsPerNeuron.HasValue
				? random.Next(options.MinStartingConnectionsPerNeuron.Value, options.StartingConnectionsPerNeuron)
				: options.StartingConnectionsPerNeuron;

			connectionsAmount = Math.Min(toChooseFrom.Count, connectionsAmount);
			
			var conns = 0;
			do {
				var n2 = random.NextFromList(toChooseFrom);
				toChooseFrom.Remove(n2);

				var weight = random.NextWeight();
				connections.Add(new NeuronConnection(neuron, n2, weight));
				conns++;
			} while (conns < connectionsAmount);
		}

		return connections;
	}

	private static T NextFromList<T>(this Random random, List<T> list) {
		// get random from list
		var index = random.Next(0, list.Count);
		return list[index];
	}
	
	private static float NextWeight(this Random random) {
		// number between -4 and 4 
		var f = (float)random.NextDouble() * 8 - 4;
		return NeuronConnection.WeightToFloat(f);
	}

	private static IEnumerable<Neuron> CreateNeurons(int count, BrainOptions options, Random random) {
		if (!options.EnabledInputNeurons.Any()) {
			throw new InvalidOperationException("No valid input neurons");
		}

		if (!options.EnabledActionNeurons.Any()) {
			throw new InvalidOperationException("No valid action neurons");
		}

		for (var i = 0; i < count; i++) {
			var id = (ushort) random.Next(0, options.MaxNeurons);
			var isInternal = random.NextDouble() < options.InternalRate;

			if (isInternal) {
				yield return new Neuron(id, NeuronType.Internal);
			}
			
			yield return GetNeuron(options, random, id);
		}
	}

	private static Neuron GetNeuron(BrainOptions options, Random r, ushort id) {
		var nonInternalType = r.NextDouble() < options.InputRate ? NeuronType.Input : NeuronType.Action;
		var mod = nonInternalType switch {
			NeuronType.Input => InputNeuron.NeuronInputAmount,
			NeuronType.Action => ActionNeuron.NeuronActionsAmount,
			_ => throw new ArgumentOutOfRangeException(nameof(nonInternalType))
		};


		while (!options.EnabledInputNeurons.Contains((InputType)(id % mod))) {
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
