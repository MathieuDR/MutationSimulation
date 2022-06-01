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
	public async Task Solve() {
		_logger.LogInformation("Solving generation {gen}", _context.Generation);
		_logger.LogInformation("Outputting in {path}", _context.BaseOutputPath);

		await Simulate();
		
		// Render gif
		if (_context.RenderGif) {
			await _renderMachine.QueueGifRender();
		}
	}

	private async Task Simulate() {
		for (int step = 0; step < _simulatorOptions.Steps; step++) {
			_logger.LogInformation("Step {step}", step);
			_logger.LogInformation("Simulating {amount} creatures", _context.Creatures.Length);
			foreach (var creature in _context.Creatures) {
				creature.Simulate(_context.World);
			}

			// Wrap around if to lessen stack
			if(_context.RenderFrames) {
				await _renderMachine.RenderFrame();
			}
		}
	}
}
