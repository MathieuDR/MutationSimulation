using Common.Models;

namespace Common.FitnessTests;

public interface IFitnessScorer {
	public (int CreatureIndex, double score)[] Score(Creature[] creatures);
}