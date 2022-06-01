namespace Common.Models;

public struct CreatureScore {
	public CreatureScore(int creatureIndex, double score) {
		CreatureIndex = creatureIndex;
		Score = score;
	}
	public int CreatureIndex { get; }
	public double Score { get; }
}
