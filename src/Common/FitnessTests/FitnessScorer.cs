using Common.FitnessTests.Parts;
using Common.Models;

namespace Common.FitnessTests;

public class FitnessScorer : IFitnessScorer {
	private readonly IFitnessPart _startPart;

	public FitnessScorer(IFitnessPart startPart) {
		_startPart = startPart;
		
	}

	public Task<CreatureScore[]> Score(Creature[] creatures) {
		var result = new CreatureScore[creatures.Length];
		for (var index = 0; index < creatures.Length; index++) {
			result[index] = new CreatureScore(index, _startPart.Score(creatures[index]));
		}

		return Task.FromResult(result);
	}
}