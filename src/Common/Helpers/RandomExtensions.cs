using Common.Models;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;

namespace Common.Helpers;

public static class RandomExtensions {
	public static Creature[] GetRandomCreatures(this Random random, int count) {
		var creatures = new Creature[count];
		var genomes = random.GetRandomGenomes(count);

		for (var i = 0; i < count; i++) {
			
		}
	}

	public static Genome[] GetRandomGenomes(this Random random, int count) {
		var genomes = new Genome[count];

		for (int i = 0; i < count; i++) {
			var connectionCount = random.Next(1, 10);
			var connections = new NeuronConnection[connectionCount];
			for (int c = 0; c < connectionCount; c++) {
				connections[c] = new NeuronConnection(random.NextNeuron(NeuronType.Input), random.NextNeuron(NeuronType.Action), random.NextFloat());
			}
			genomes[i] = new Genome(connections);
		}

		return genomes;
	}

	private static float NextFloat(this Random random) {
		// return float between min float and max float
		double mantissa = (random.NextDouble() * 2.0) - 1.0;
		// choose -149 instead of -126 to also generate subnormal floats (*)
		double exponent = Math.Pow(2.0, random.Next(-126, 128));
		return (float)(mantissa * exponent);
	}

	public static Neuron NextNeuron(this Random random, NeuronType nonInternalType) {
		var type = random.NextDouble() > 0.5 ? NeuronType.Internal : nonInternalType;
		var id = random.Next(1, Neuron.MaxId);
		
		return new Neuron((ushort)id, type);
	}
}
