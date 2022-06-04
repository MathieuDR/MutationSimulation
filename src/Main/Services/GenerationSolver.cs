using Common.FitnessTests;
using Common.Models;
using Common.Models.Genetic;
using Common.Models.Genetic.Components;
using Common.Models.Options;
using Graphics.Helpers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Main.Services;

public class GenerationSolver {
	private readonly ILogger<GenerationSolver> _logger;
	private readonly GenerationContext _context;
	private readonly Simulator _simulator;
	private readonly IFitnessScorer _scorer;
	private readonly Mutator _mutator;
	private readonly RenderOptions _renderOptions;

	public GenerationSolver(ILogger<GenerationSolver> logger, GenerationContext context, Simulator simulator, IFitnessScorer scorer, Mutator mutator, IOptionsSnapshot<RenderOptions> renderOptions) {
		_logger = logger;
		_context = context;
		_simulator = simulator;
		_scorer = scorer;
		_mutator = mutator;
		_renderOptions = renderOptions.Value;
	}

	public async Task<Genome[]> SolveGeneration() {
		_logger.LogTrace("Starting generation {generation}", _context.Generation);
		await _simulator.Simulate();
		_logger.LogTrace("Simulation complete");
		_logger.LogTrace("Scoring generation {generation}", _context.Generation);
		var scores = (await _scorer.Score(_context.Creatures)).OrderByDescending(x => x.Score).ToArray();
		_logger.LogTrace("Scoring complete");
		var half = _context.Creatures.Length / 2;

		var topDogs = new Creature[half];
		for (var i = 0; i < half; i++) {
			topDogs[i] = _context.Creatures[scores[i].CreatureIndex];
		}

		await SaveBrains(topDogs.Select(x=> x.Brain));
		
		var newGeneration = await _mutator.Mutate(topDogs);

		return newGeneration;
	}

	private Task SaveBrains(IEnumerable<Brain> brainsEnumerable) {
		if(!_renderOptions.OutputTopBrains && !_renderOptions.OutputAllBrains) {
			return Task.CompletedTask;
		}
		
		var brains = brainsEnumerable.ToArray();
		var path = Path.Combine(_context.BaseOutputPath, "brains/");

		var toPrint = _renderOptions.OutputAllBrains ? brains.Length : Math.Min(brains.Length, _renderOptions.TopBrainsAmount);
		
		for (var i = 0; i < toPrint; i++) {
			var brain = brains[i];
			var fileName = $"brain_{i}.dot";
			brain.CreateDotFile(path, fileName);
		}
		
		return Task.CompletedTask;
	}
}
