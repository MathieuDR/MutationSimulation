using Common.Helpers;
using Common.Models;
using Common.Models.Genetic.Components;
using Common.Models.Options;
using Common.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Common.Factories;

public class CreatureFactory {
	private readonly ILogger<CreatureFactory> _logger;
	private readonly Random _random;
	private readonly WorldOptions _worldOptions;
	private readonly SimulatorOptions _simulatorOptions;
	private readonly CreatureOptions _creatureOptions;

	public CreatureFactory(ILogger<CreatureFactory> logger, IRandomProvider randomProvider, IOptionsSnapshot<WorldOptions> worldOptions, IOptionsSnapshot<SimulatorOptions> simulatorOptions, IOptionsSnapshot<CreatureOptions> creatureOptions, GenomeFactory genomeFactory) {
		_logger = logger;
		_random = randomProvider.GetRandom();
		_worldOptions = worldOptions.Value;
		_simulatorOptions = simulatorOptions.Value;
		_creatureOptions = creatureOptions.Value;
	}

	public IEnumerable<Creature> Create(Genome[] genomes, Line[] walls) {
		_logger.LogTrace("Creating {length} creatures", genomes.Length);
		var currentPositions = new List<(Vector position, int radius)>();
		
		foreach (var genome in genomes) {
			var radius = _random.Next(_creatureOptions.MinRadius, _creatureOptions.MaxRadius);
			var position = _simulatorOptions.ValidateStartPositions
				? _random.GetValidPosition(radius, _worldOptions.Width, _worldOptions.Height, currentPositions, walls)
				: _random.GetRandomPosition(_worldOptions.Width, _worldOptions.Height);

			currentPositions.Add((position, radius));
			var color = GenerateColorFromGenome(genome);
			yield return new Creature(genome, position, radius, color, _random);
		}
	}

	private Color GenerateColorFromGenome(Genome genome) {
		return new Color(_random);
	}
	
	

}
