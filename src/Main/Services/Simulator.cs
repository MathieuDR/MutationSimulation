using Common.Models;
using Common.Models.Options;
using Graphics;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Main.Services;

public class Simulator {
	private readonly ILogger<Simulator> _logger;
	private readonly GenerationContext _context;
	private readonly RenderMachine _renderMachine;
	private readonly SimulatorOptions _simulatorOptions;

	public Simulator(ILogger<Simulator> logger, GenerationContext context, IOptionsSnapshot<SimulatorOptions> simulatorOptions, RenderMachine renderMachine) {
		_logger = logger;
		_context = context;
		_renderMachine = renderMachine;
		_simulatorOptions = simulatorOptions.Value;
	}
	public async Task Simulate() {
		_logger.LogInformation("Solving generation {gen}", _context.Generation);
		_logger.LogInformation("Outputting in {path}", _context.BaseOutputPath);

		// Simulates
		await DoSimulation();

		// Render gif
		if (_context.RenderGif) {
			await _renderMachine.QueueGifRender();
		}
	}

	private async Task DoSimulation() {
		for (int step = 0; step < _simulatorOptions.Steps; step++) {
			foreach (var creature in _context.Creatures) {
				creature.Simulate(_context.World);
			}
			
			_context.NextTick();

			// Wrap around if to lessen stack
			if (_context.RenderFrames) {
				await _renderMachine.RenderFrame();
			}
		}
	}
}
