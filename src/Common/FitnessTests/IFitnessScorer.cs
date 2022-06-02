using Common.Models;

namespace Common.FitnessTests;

public interface IFitnessScorer {
	public Task<CreatureScore[]> Score(Creature[] creatures);
}