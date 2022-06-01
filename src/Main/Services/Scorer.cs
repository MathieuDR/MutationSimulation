using Common.Models;
using Common.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Main.Services; 

public class Scorer {
	public Scorer(ILogger<Scorer> logger, GenerationContext context, IOptionsSnapshot<FitnessOptions> fitnessOptions) {
		
	}
}
