namespace Common.Services;

public interface IRandomProvider {
	public Random GetRandom();
	// public Random GetRandomForId(int id);
	public void SetSeed(string seed);
	public void SetSeed(int seed);
	// public void SetSeed(int id, string seed);
	// public void SetSeed(int id, int seed);
}