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
	private int _mutationCount = 0;

	public Mutator(ILogger<Mutator> logger,IOptionsSnapshot<SimulatorOptions> options, IRandomProvider randomProvider) {
		_logger = logger;
		_mutationRate = options.Value.MutationRate;
		_random = randomProvider.GetRandom();
	}

	public Task<Genome[]> Mutate(Creature[] creatures) {
		var doubled = DoubleGenomes(creatures.Select(x => x.Genome).ToArray());
		var mutated = doubled.Select(MutateGenome).ToArray();
		_logger.LogTrace("Mutated {count} genomes", mutated.Length);
		_logger.LogInformation("Flipped {bits} bits with a {rate} mutation rate", _mutationCount, _mutationRate);
		
		return Task.FromResult(mutated);
	}

	private Genome MutateGenome(Genome g) {
		var bytes = g.GetBytes();
		var flipBytes = MutateBytesByFlip(bytes);
		return Genome.FromBytes(flipBytes);
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
				_mutationCount++;
			}
		}
		return mutatedBytes;
	}

	public Genome[] DoubleGenomes(Genome[] genomes) {
		var result = new Genome[genomes.Length * 2];
		
		// duplicate array
		for (var i = 0; i < genomes.Length; i++) {
			result[i] = genomes[i];
			result[i + genomes.Length] = genomes[i];
		}

		return result;
	}

}
