using Common.Models;

namespace Common.FitnessTests;

public interface IFitnessScorer {
	public CreatureScore[] Score(Creature[] creatures);
}