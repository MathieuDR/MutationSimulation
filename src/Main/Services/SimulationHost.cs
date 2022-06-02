using Common.Models.Genetic.Components;
using Common.Models.Options;
using Graphics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Main.Services;

public class SimulationHost : IHostedService {
	private readonly IHostApplicationLifetime _applicationLifetime;
	private readonly ILogger<SimulationHost> _logger;
	private readonly SimulatorOptions _options;
	private readonly IServiceProvider _serviceProvider;

	public SimulationHost(ILogger<SimulationHost> logger, IHostApplicationLifetime applicationLifetime,
		IServiceProvider serviceProvider, IOptionsSnapshot<SimulatorOptions> options) {
		_logger = logger;
		_applicationLifetime = applicationLifetime;
		_serviceProvider = serviceProvider;
		_options = options.Value;
	}

	public Task StartAsync(CancellationToken cancellationToken) {
		_logger.LogInformation("Starting the host");
		Task.Run(() => {
			try {
				return StartSimulation(cancellationToken);
			} catch (Exception e) {
				_logger.LogError(e, "Error in sim");
				return Task.FromException(e);
			}
		}, cancellationToken);
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation("Stopping the host");
		return Task.CompletedTask;
	}

	private async Task StartSimulation(CancellationToken cancellationToken) {
		var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
		var genomes = Array.Empty<Genome>();

		for (var gen = 0; gen < _options.Generations; gen++) {
			try {
				using var scope = scopeFactory.CreateScope();
				CreateContext(scope, gen, genomes, cancellationToken);
				genomes = await SolveGeneration(scope);
			} catch (Exception e) {
				_logger.LogError(e, "Error in sim");
			}
		}

		_applicationLifetime.StopApplication();
	}

	private async Task<Genome[]> SolveGeneration(IServiceScope scope) {
		var solver = scope.ServiceProvider.GetRequiredService<GenerationSolver>();
		return await solver.SolveGeneration();
	}

	private void CreateContext(IServiceScope scope, int generation, Genome[] genomes, CancellationToken cancellationToken) {
		var contextProvider = scope.ServiceProvider.GetRequiredService<ContextProvider>();
		contextProvider.Initialize(generation, genomes, cancellationToken);
	}
}
