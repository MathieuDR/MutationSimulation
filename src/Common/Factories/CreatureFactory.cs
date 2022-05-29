using Common.Helpers;
using Common.Models;
using Common.Models.Genetic.Components;
using Common.Models.Options;
using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Factories;

internal static class CreatureFactory {
	public static IEnumerable<Creature> CreateCreatures(IServiceProvider serviceProvider, int count, IEnumerable<Line>? walls = null) {
		var creatureOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<CreatureOptions>>().Value;
		var simulatorOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<SimulatorOptions>>().Value;
		var worldOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<WorldOptions>>().Value;
		var validatePosition = simulatorOptions.ValidateStartPositions;
		var random = serviceProvider.GetRequiredService<IRandomProvider>().GetRandom();
		var wallArray = walls?.ToArray() ?? Array.Empty<Line>();
		
		var genomes = GenomeFactory.CreateGenomes(serviceProvider, count);
		var currentPositions = new List<(Vector position, int radius)>();
		using var genomeEnumerator = genomes.GetEnumerator();
		
		for (int i = 0; i < count; i++) {
			genomeEnumerator.MoveNext();
			var genome = genomeEnumerator.Current;
			var radius = random.Next(creatureOptions.MinRadius, creatureOptions.MaxRadius);
			var position = validatePosition
				? random.GetValidPosition(radius, worldOptions.Width, worldOptions.Height, currentPositions, wallArray)
				: random.GetRandomPosition(worldOptions.Width, worldOptions.Height);
			var color = GenerateColorFromGenome(genome, random);
			
			
			yield return new Creature(genome, position, radius, color, random);
		}
	}

	private static Color GenerateColorFromGenome(Genome genome, Random random) {
		// Todo!
		return new Color(random);
	}
}
