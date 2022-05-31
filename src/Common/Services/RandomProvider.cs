using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Common.Services;

public class RandomProvider : IRandomProvider {
	private readonly ILogger<RandomProvider> _logger;

	private Random _random = new();

	public RandomProvider(ILogger<RandomProvider> logger) => _logger = logger;

	public string? SeedString { get; private set; }
	public int? Seed { get; private set; }
	public Random GetRandom() => _random;

	public void SetSeed(string seed) {
		_logger.LogTrace("Setting seed for shared random to {seed}", seed);
		SeedString = seed;
		SetSeed(StringToInt(seed));
	}

	public void SetSeed(int seed) {
		_logger.LogTrace("Setting seed for shared random to {seed}", seed);
		Seed = seed;
		_random = new Random(seed);
	}

	private static int StringToInt(string str) {
		using var algorithm = SHA1.Create();
		var bytes = Encoding.UTF8.GetBytes(str);
		return BitConverter.ToInt32(algorithm.ComputeHash(bytes));
	}
}
