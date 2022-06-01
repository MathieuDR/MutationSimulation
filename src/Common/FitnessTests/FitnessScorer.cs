using Common.FitnessTests.Parts;
using Common.Models;

namespace Common.FitnessTests;

internal class FitnessScorer : IFitnessScorer {
	private readonly IFitnessPart _startPart;

	public FitnessScorer(IFitnessPart startPart) {
		_startPart = startPart;
		
	}

	public CreatureScore[] Score(Creature[] creatures) {
		var result = new CreatureScore[creatures.Length];
		for (var index = 0; index < creatures.Length; index++) {
			result[index] = new CreatureScore(index, _startPart.Score(creatures[index]));
		}

		return result;
	}
}