using Common.FitnessTests;
using Common.Models;
using Common.Models.Genetic.Components;
using Microsoft.Extensions.Logging;

namespace Main.Services;

public class GenerationSolver {
	private readonly ILogger<GenerationSolver> _logger;
	private readonly GenerationContext _context;
	private readonly Simulator _simulator;
	private readonly IFitnessScorer _scorer;
	private readonly Mutator _mutator;
	public GenerationSolver(ILogger<GenerationSolver> logger, GenerationContext context, Simulator simulator, IFitnessScorer scorer, Mutator mutator) {
		_logger = logger;
		_context = context;
		_simulator = simulator;
		_scorer = scorer;
		_mutator = mutator;
	}

	public async Task<Genome[]> SolveGeneration() {
		_logger.LogInformation("Starting generation {generation}", _context.Generation);
		await _simulator.Simulate();
		_logger.LogInformation("Simulation complete");
		_logger.LogInformation("Scoring generation {generation}", _context.Generation);
		var scores = (await _scorer.Score(_context.Creatures)).OrderByDescending(x => x.Score).ToArray();
		_logger.LogInformation("Scoring complete");
		var half = _context.Creatures.Length / 2;

		var topDogs = new Creature[half];
		// fill them
		for (var i = 0; i < half; i++) {
			topDogs[i] = _context.Creatures[scores[i].CreatureIndex];
		}
		
		var newGeneration = await _mutator.Mutate(topDogs);

		return newGeneration;
	}
}
