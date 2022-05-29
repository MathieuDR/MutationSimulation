using Common.Factories;
using Common.Models.Options;
using Common.Simulator;
using Graphics.Helpers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Main.Services;

public class SimulationHost : IHostedService {
	private readonly IHostApplicationLifetime _applicationLifetime;
	private readonly IServiceProvider _serviceProvider;
	private readonly IOptionsMonitor<BrainOptions> _brainMonitor;
	private readonly IOptionsMonitor<CreatureOptions> _creatureMonitor;
	private readonly IOptionsMonitor<RandomOptions> _randomMonitor;
	private readonly IOptionsMonitor<RenderOptions> _renderMonitor;
	private readonly IOptionsMonitor<SimulatorOptions> _simulatorMonitor;
	private readonly IOptionsMonitor<WorldOptions> _worldMonitor;
	private readonly ILogger<SimulationHost> _logger;

	public SimulationHost(ILogger<SimulationHost> logger, IHostApplicationLifetime applicationLifetime,
		IServiceProvider serviceProvider, IOptionsMonitor<BrainOptions> brainMonitor, 
		IOptionsMonitor<CreatureOptions> creatureMonitor, 
		IOptionsMonitor<RandomOptions> randomMonitor, IOptionsMonitor<RenderOptions> renderMonitor, 
		IOptionsMonitor<SimulatorOptions> simulatorMonitor, IOptionsMonitor<WorldOptions> worldMonitor
		) {
		_logger = logger;
		_applicationLifetime = applicationLifetime;
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
			RandomProvider.SetSeed(_randomMonitor.CurrentValue.Seed);
		}
		
		//LoopModus(cancellationToken);

		var world = WorldFactory.CreateWorld(_serviceProvider);
		foreach (var creature in world.Creatures) {
			creature.Brain.CreateDotFile(fileName: $"{creature.Id}.dot");
		}
		
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
		T opts = options.CurrentValue;
		_logger.LogInformation("{T} Options: {@opts}", typeof(T), opts);
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation("Stopping the host");
		return Task.CompletedTask;
	}
}
