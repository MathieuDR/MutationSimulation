using Common.FitnessTests.Parts;
using Common.Models;

namespace Common.FitnessTests;

internal class FitnessScorer : IFitnessScorer {
	private readonly IFitnessPart[] _parts;

	public FitnessScorer(IEnumerable<IFitnessPart> parts) {
		_parts = parts.ToArray();
	}

	public (int CreatureIndex, double score)[] Score(Creature[] creatures) {
		var result = new (int index, double score)[creatures.Length];
		for (var index = 0; index < creatures.Length; index++) {
			var creature = creatures[index];
			var score = _parts.Sum(part => part.Score(creature));
			result[index] = (index, score);
		}

		return result;
	}
}