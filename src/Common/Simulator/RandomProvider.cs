using System.Runtime.Versioning;
using System.Security.Cryptography;
using System.Text;

namespace Common.Simulator; 

public class RandomProvider {
	private static int? _seed;
	private static readonly Lazy<Random> Random = new Lazy<Random>(() => _seed.HasValue ? new Random(_seed.Value) : new Random());
	
	public static Random GetRandom() {
		return Random.Value;
	}
	
	public static void SetSeed(int seed) {
		_seed = seed;
		Console.WriteLine("Seed: {0}", _seed);
	}

	public static void SetSeed(string seed) {
		using var algo = SHA1.Create();
		_seed= BitConverter.ToInt32(algo.ComputeHash(Encoding.UTF8.GetBytes(seed)));
		Console.WriteLine("Seed: {0}", _seed);
	}
}
