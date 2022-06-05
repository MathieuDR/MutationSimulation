using Common.FitnessTests.Parts;
using Common.Models;
using Microsoft.Extensions.Logging;

namespace Common.FitnessTests;

public class FitnessScorer : IFitnessScorer {
	private readonly ILogger<FitnessScorer> _logger;
	private readonly IFitnessPart _startPart;

	public FitnessScorer(IFitnessPart startPart, ILogger<FitnessScorer> logger) {
		_startPart = startPart;
		_logger = logger;
	}

	public Task<CreatureScore[]> Score(Creature[] creatures) {
		var result = new CreatureScore[creatures.Length];
		var avgScore = 0d;
		for (var index = 0; index < creatures.Length; index++) {
			result[index] = new CreatureScore(index, _startPart.Score(creatures[index]));
			avgScore += result[index].Score;
		}
		
		_logger.LogInformation("Average score of {creatures} creatures: {score}",creatures.Length, avgScore / creatures.Length);
		return Task.FromResult(result);
	}
}
