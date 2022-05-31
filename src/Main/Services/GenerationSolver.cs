using Common.Models;
using Microsoft.Extensions.Logging;

namespace Main.Services; 

public class GenerationSolver {
	private readonly ILogger<GenerationSolver> _logger;
	private readonly GenerationContext _context;

	public GenerationSolver(ILogger<GenerationSolver> logger,GenerationContext context) {
		_logger = logger;
		_context = context;
	}
	public Task Solve() {
		_logger.LogInformation("Solving generation {gen}", _context.Generation);
		_logger.LogInformation("Outputting in {path}", _context.BaseOutputPath);
		return Task.CompletedTask;
	}
}
