using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Common.Services;

public class RandomProvider : IRandomProvider {
	private readonly ILogger<RandomProvider> _logger;

	// private readonly Dictionary<int, Random> _randomDictionary = new();
	private Random _random = new();
	// private int? _seed;

	public RandomProvider(ILogger<RandomProvider> logger) => _logger = logger;

	public Random GetRandom() => _random;

	// public Random GetRandomForId(int id) {
	// 	if (!_randomDictionary.TryGetValue(id, out var random)) {
	// 		random = new Random();
	// 		_randomDictionary.Add(id, random);
	// 	}
	//
	// 	_logger.LogTrace("Returning random for ID: {id}", id);
	// 	return random;
	// }

	public void SetSeed(string seed) {
		_logger.LogTrace("Setting seed for shared random to {seed}", seed);
		SetSeed(StringToInt(seed));
	}

	public void SetSeed(int seed) {
		_logger.LogTrace("Setting seed for shared random to {seed}", seed);
		_random = new Random(seed);
	}

	// public void SetSeed(int id, string seed) {
	// 	_logger.LogTrace("Setting seed for {id} random to {seed}", id, seed);
	// 	SetSeed(id, StringToInt(seed));
	// }

	// public void SetSeed(int id, int seed) {
	// 	if (_randomDictionary.ContainsKey(id)) {
	// 		_logger.LogWarning("Cannot set seed for {id} since it already exists", id);
	// 		return;
	// 	}
	//
	// 	_logger.LogTrace("Setting seed for {id} random to {seed}", id, seed);
	// 	_randomDictionary.Add(id, new Random(seed));
	// }

	private static int StringToInt(string str) {
		using var algorithm = SHA1.Create();
		var bytes = Encoding.UTF8.GetBytes(str);
		return BitConverter.ToInt32(algorithm.ComputeHash(bytes));
	}
}
