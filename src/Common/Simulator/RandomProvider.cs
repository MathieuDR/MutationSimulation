using System.Runtime.Versioning;

namespace Common.Simulator; 

public class RandomProvider {
	private static int? _seed;
	private static readonly Lazy<Random> Random = new Lazy<Random>(() => _seed.HasValue ? new Random(_seed.Value) : new Random());
	
	public static Random GetRandom() {
		return Random.Value;
	}
	
	public static void SetSeed(int seed) {
		_seed = seed;
	}
}
