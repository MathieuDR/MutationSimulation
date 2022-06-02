using Common.Models;
using Common.Models.Genetic.Components;
using Microsoft.Extensions.Logging;

namespace Main.Services; 

public class Mutator {
	private readonly ILogger<Mutator> _logger;
	private readonly GenerationContext _context;

	public Mutator(ILogger<Mutator> logger, GenerationContext context) {
		_logger = logger;
		_context = context;
	}

	public Task<Genome[]> Mutate(Creature[] creatures) {
		var result = new Genome[creatures.Length * 2];
		
		// duplicate array
		for (var i = 0; i < creatures.Length; i++) {
			result[i] = creatures[i].Genome;
			result[i + creatures.Length] = creatures[i].Genome;
		}
		
		return Task.FromResult(result);
	}
}
