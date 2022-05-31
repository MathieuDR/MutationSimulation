namespace Common.Services;

public interface IRandomProvider {
	public string? SeedString { get; }
	public int? Seed { get; }
	public Random GetRandom();
	public void SetSeed(string seed);
	public void SetSeed(int seed);
}