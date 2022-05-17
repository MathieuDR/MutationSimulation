using Common.Models;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;

namespace Common.Helpers;

public static class RandomExtensions {
	private const int MinRadius = 5;
	private const int MaxRadius = 10;
	private const double InternalConnChance = 0.25;
	private const int MaxId = 15;
	private const int MaxConnections = 50;
	private const int MinConnections = 10;
	public static Creature[] GetRandomCreatures(this Random random, int count, int worldX, int worldY) {
		var creatures = new Creature[count];
		var genomes = random.GetRandomGenomes(count);

		for (var i = 0; i < count; i++) {
			var randPos = random.GetRandomPosition(worldX, worldY);
			creatures[i] = new Creature(genomes[i], randPos, random.Next(MinRadius, MaxRadius), new Color(random));
		}

		return creatures;
	}
	
	private static Position GetRandomPosition(this Random random, int x, int y) {
		return new Position(random.Next(x+1), random.Next(y+1));
	}

	public static Genome[] GetRandomGenomes(this Random random, int count) {
		var genomes = new Genome[count];

		for (int i = 0; i < count; i++) {
			var connectionCount = random.Next(MinConnections, MaxConnections);
			var connections = new NeuronConnection?[connectionCount];
			for (int c = 0; c < connectionCount; c++) {
				var n1 = random.NextNeuron(NeuronType.Input);
				var n2 = random.NextNeuron(NeuronType.Action);
				
				if(connections.Any(x => x is not null && x.Source == n1 && x.Target == n2)) {
					// redo
					c--;
					continue;
				}

				if(c < connectionCount - 1) {
					if(random.NextDouble() < InternalConnChance) {
						var n3 = random.NextNeuron(NeuronType.Internal);
						connections[c] = new NeuronConnection(n1, n3, random.NextFloat());
						connections[c+1] = new NeuronConnection(n3, n2, random.NextFloat());
						c++;
						continue;
					}
				}
				
				connections[c] = new NeuronConnection(n1, n2, random.NextFloat());
			}
			genomes[i] = new Genome(connections.Where(x=> x is not null).Cast<NeuronConnection>().ToArray());
		}

		return genomes;
	}

	private static float NextFloat(this Random random) {
		// number between -4 and 4 
		var f = (float)random.NextDouble() * 8 - 4;
		return NeuronConnection.WeightToFloat(f);
		
		// // return float between min float and max float
		// double mantissa = (random.NextDouble() * 2.0) - 1.0;
		// // choose -149 instead of -126 to also generate subnormal floats (*)
		// double exponent = Math.Pow(2.0, random.Next(-126, 128));
		// return (float)(mantissa * exponent);
	}

	public static Neuron NextNeuron(this Random random, NeuronType nonInternalType) {
		var type = random.NextDouble() > 0.5 ? NeuronType.Internal : nonInternalType;
		var id = (ushort)random.Next(1, MaxId);

		return type switch {
			NeuronType.Input => new InputNeuron(id),
			NeuronType.Action => new ActionNeuron(id),
			NeuronType.Internal => new Neuron(id, NeuronType.Internal),
			NeuronType.Memory => throw new NotSupportedException(),
			_ => throw new ArgumentOutOfRangeException()
		};
	}
}
