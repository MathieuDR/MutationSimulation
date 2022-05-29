using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using Common.Models.Options;
using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Factories;

internal static class GenomeFactory {
	public static IEnumerable<Genome> CreateGenomes(IServiceProvider serviceProvider, int count) {
		var brainOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<BrainOptions>>().Value;
		var random = serviceProvider.GetRequiredService<IRandomProvider>().GetRandom();

		// var logger = serviceProvider.GetRequiredService<ILogger<Genome>>();

		// var count2 = 0;
		// var internals = 0;
		// var actions = 0;
		// var inputs = 0;
		
		for (var i = 0; i < count; i++) {
			var neuronsAmount = brainOptions.MinStartNeurons.HasValue
				? random.Next(brainOptions.MinStartNeurons.Value, brainOptions.StartNeurons + 1)
				: brainOptions.StartNeurons;

			var neurons = CreateNeurons(neuronsAmount, brainOptions, random).ToArray();
			
			//
			// count2 += neurons.Count();
			// internals += neurons.Count(x => x.NeuronType == NeuronType.Internal);
			// actions+= neurons.Count(x => x.NeuronType == NeuronType.Action);
			// inputs+= neurons.Count(x => x.NeuronType == NeuronType.Input);
			//
			
			var connections = CreateConnections(neurons, brainOptions, random);


			// if (i + 1 >= count) {
			// 	logger.LogInformation("from factory");
			// 	logger.LogInformation("{neurons} total neurons", count2);
			// 	logger.LogInformation("{neurons} input neurons ({perc}%)", inputs, ((double)inputs/count2));
			// 	logger.LogInformation("{neurons} action neurons ({perc}%)", actions, ((double)actions/count2));
			// 	logger.LogInformation("{neurons} internal neurons ({perc}%)", internals, ((double)internals/count2));
			// }

			yield return new Genome(connections.ToArray());
		}
		
		
		yield break;
	}


	private static IEnumerable<NeuronConnection> CreateConnections(IEnumerable<Neuron> neurons, BrainOptions options, Random random) {
		var connections = new List<NeuronConnection>();
		var neuronArray = neurons.ToArray();
		//var withoutActions = neuronArray.Where(x => x.NeuronType != NeuronType.Action);
		var withoutInputs = neuronArray.Where(x => x.NeuronType != NeuronType.Input).ToArray();
		
		foreach (var neuron in neuronArray) {
			if(neuron.NeuronType == NeuronType.Action)
				continue;
			
			var toChooseFrom = withoutInputs.ToList();

			var connectionsAmount = options.MinStartingConnectionsPerNeuron.HasValue
				? random.Next(options.MinStartingConnectionsPerNeuron.Value, options.StartingConnectionsPerNeuron + 1)
				: options.StartingConnectionsPerNeuron;

			connectionsAmount = Math.Min(toChooseFrom.Count, connectionsAmount);
			
			var conns = 0;
			while (conns++ < connectionsAmount) {
				var n2 = random.NextFromList(toChooseFrom);
				toChooseFrom.Remove(n2);

				var weight = random.NextWeight();
				connections.Add(new NeuronConnection(neuron, n2, weight));
				//conns++;
			} 
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
			var randNum = random.NextDouble();
			var isInternal = randNum < options.InternalRate;

			if (isInternal) {
				yield return new Neuron(id, NeuronType.Internal);
				continue;
			}
			
			yield return GetNeuron(options, random, id);
		}
	}

	private static Neuron GetNeuron(BrainOptions options, Random r, ushort id) {
		var randNum = r.NextDouble();
		var nonInternalType = randNum < options.InputRate ? NeuronType.Input : NeuronType.Action;
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
