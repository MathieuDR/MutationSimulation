using Common.Models;
using Common.Models.Genetic.Components;
using Common.Models.Options;
using Common.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Main.Services; 

public class Mutator {
	private readonly ILogger<Mutator> _logger;
	private readonly Random _random;
	private readonly double _mutationRate;
	private int _flippedCount = 0;
	private int _addedBytes = 0;

	public Mutator(ILogger<Mutator> logger,IOptions<SimulatorOptions> options, IRandomProvider randomProvider) {
		_logger = logger;
		_mutationRate = options.Value.MutationRate;
		_random = randomProvider.GetRandom();
	}

	public Task<OldGenome[]> Mutate(Creature[] creatures) {
		var doubled = DoubleGenomes(creatures.Select(x => x.Genome).ToArray());
		var mutated = doubled.Select(MutateGenome).ToArray();
		_logger.LogTrace("Mutated {count} genomes", mutated.Length);
		_logger.LogInformation("Flipped {bits} bits with a {rate} mutation rate", _flippedCount, _mutationRate);
		_logger.LogInformation("Added {bytes} bytes with a {rate} mutation rate", _addedBytes, _mutationRate);
		
		return Task.FromResult(mutated);
	}

	private OldGenome MutateGenome(OldGenome g) {
		var bytes = g.GetBytes();
		var flipBytes = MutateBytesByFlip(bytes);
		var mutated = AddRandomByte(flipBytes);
		return OldGenome.FromBytes(mutated);
	}

	private byte[] AddRandomByte(byte[] flipBytes) {
		var r = _random.NextDouble();

		if (!(r < _mutationRate)) {
			return flipBytes;
		}

		var buffer = new byte[1];
		_random.NextBytes(buffer);
		var result = new byte[flipBytes.Length + 1];
		flipBytes.CopyTo(result, 0);
		result[^1] = buffer[0];
		_addedBytes++;
		return result;

	}

	private byte[] MutateBytesByFlip(byte[] bytes) {
		var mutatedBytes = new byte[bytes.Length];
		for (var i = 0; i < bytes.Length; i++) {
			mutatedBytes[i] = bytes[i];
			for(var j = 0; j < 8; j++) {
				if (!(_random.NextDouble() < _mutationRate)) {
					continue;
				}

				mutatedBytes[i] ^= (byte)(1 << j);
				_flippedCount++;
			}
		}
		return mutatedBytes;
	}

	public OldGenome[] DoubleGenomes(OldGenome[] genomes) {
		var result = new OldGenome[genomes.Length * 2];
		
		// duplicate array
		for (var i = 0; i < genomes.Length; i++) {
			result[i] = genomes[i];
			result[i + genomes.Length] = genomes[i];
		}

		return result;
	}

}
