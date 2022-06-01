using Common.Models;
using Microsoft.Extensions.Logging;

namespace Main.Services; 

public class Mutator {
	private readonly GenerationContext _context;

	public Mutator(ILogger<Mutator> logger, GenerationContext context) {
		_context = context;
	}

	public Creature[] Mutate(Creature[] creatures) {
		return Array.Empty<Creature>(); 
	}
}
