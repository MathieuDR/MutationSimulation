using Common.Factories;
using Common.Models.Genetic.Components.Neurons;
using Common.Models.Options;
using Common.Services;
using Graphics.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Main.Services;

public class SimulationHost : IHostedService {
	private readonly IHostApplicationLifetime _applicationLifetime;
	private readonly IOptionsMonitor<BrainOptions> _brainMonitor;
	private readonly IOptionsMonitor<CreatureOptions> _creatureMonitor;
	private readonly ILogger<SimulationHost> _logger;
	private readonly IOptionsMonitor<RandomOptions> _randomMonitor;
	private readonly IRandomProvider _randomProvider;
	private readonly IOptionsMonitor<RenderOptions> _renderMonitor;
	private readonly IServiceProvider _serviceProvider;
	private readonly IOptionsMonitor<SimulatorOptions> _simulatorMonitor;
	private readonly IOptionsMonitor<WorldOptions> _worldMonitor;

	public SimulationHost(ILogger<SimulationHost> logger, IHostApplicationLifetime applicationLifetime,
		IRandomProvider randomProvider,
		IServiceProvider serviceProvider, IOptionsMonitor<BrainOptions> brainMonitor,
		IOptionsMonitor<CreatureOptions> creatureMonitor,
		IOptionsMonitor<RandomOptions> randomMonitor, IOptionsMonitor<RenderOptions> renderMonitor,
		IOptionsMonitor<SimulatorOptions> simulatorMonitor, IOptionsMonitor<WorldOptions> worldMonitor
	) {
		_logger = logger;
		_applicationLifetime = applicationLifetime;
		_randomProvider = randomProvider;
		_serviceProvider = serviceProvider;
		_brainMonitor = brainMonitor;
		_creatureMonitor = creatureMonitor;
		_randomMonitor = randomMonitor;
		_renderMonitor = renderMonitor;
		_simulatorMonitor = simulatorMonitor;
		_worldMonitor = worldMonitor;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		_logger.LogInformation("Starting the host");
		if (_randomMonitor.CurrentValue.UseSeed) {
			_randomProvider.SetSeed(_randomMonitor.CurrentValue.Seed);
		}

		//LoopModus(cancellationToken);
		var world = WorldFactory.CreateWorld(_serviceProvider);
		foreach (var creature in world.Creatures) {
			creature.Brain.CreateDotFile(fileName: $"{creature.Id}.dot");
		}

		var neurons = world.Creatures.SelectMany(x => x.Brain.SortedNeurons.Where(x=> x.NeuronType != NeuronType.Memory)).ToArray();

		var count = neurons.Count();
		var internals = neurons.Count(x => x.NeuronType == NeuronType.Internal);
		var action = neurons.Count(x => x.NeuronType == NeuronType.Action);
		var input = neurons.Count(x => x.NeuronType == NeuronType.Input);

		var shookies = world.Creatures.Count(x => !x.Brain.SortedNeurons.Any());
		
		_logger.LogInformation("{neurons} total neurons", count);
		_logger.LogInformation("{neurons} input neurons ({perc}%)", input, ((double)input/count));
		_logger.LogInformation("{neurons} action neurons ({perc}%)", action, ((double)action/count));
		_logger.LogInformation("{neurons} internal neurons ({perc}%)", internals, ((double)internals/count));
		_logger.LogInformation("{nobrainers} shookies ({perc}%)", shookies, ((double)shookies/world.Creatures.Count()));
		// _logger.LogInformation("{neurons} memory neurons ({perc}%)", mem, ((double)mem/count));
		// _logger.LogInformation("{neurons} internal+mem neurons ({perc}%)", mem+internals, ((double)(mem+internals)/count));


		_applicationLifetime.StopApplication();
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation("Stopping the host");
		return Task.CompletedTask;
	}


	private async Task LoopModus(CancellationToken cancellationToken) {
		try {
			while (true) {
				LogCurrent(_brainMonitor);
				LogCurrent(_creatureMonitor);
				LogCurrent(_randomMonitor);
				LogCurrent(_renderMonitor);
				LogCurrent(_simulatorMonitor);
				LogCurrent(_worldMonitor);
				await Task.Delay(5000, cancellationToken);
			}
		} catch (Exception e) {
			_logger.LogError(e, "this is what happens..");
		}
	}

	private void LogCurrent<T>(IOptionsMonitor<T> options) {
		var opts = options.CurrentValue;
		_logger.LogInformation("{T} Options: {@opts}", typeof(T), opts);
	}
}
