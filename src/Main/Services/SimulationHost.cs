using Common.FitnessTests.Parts;
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
	private readonly IServiceProvider _serviceProvider;

	public SimulationHost(ILogger<SimulationHost> logger, IHostApplicationLifetime applicationLifetime,
		IServiceProvider serviceProvider) {
		_logger = logger;
		_applicationLifetime = applicationLifetime;
		_serviceProvider = serviceProvider;
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

	public async Task StartSimulation(CancellationToken cancellationToken) {

		var scopeFactory = _serviceProvider.GetRequiredService<IServiceScopeFactory>();
		
		var maxGenerations = 11;
		var gifTasks = new List<Task>();
		var render = 0;
		for (var gen = 0; gen < maxGenerations; gen++) {
			try {
				using var scope = scopeFactory.CreateScope();
				CreateContext(scope, gen);
				await SolveGeneration(scope);

			} catch (Exception e) {
				_logger.LogError(e, "Error in sim");
			}

			// _logger.LogInformation("Starting gen {gen}", gen);
			// var world = WorldFactory.CreateWorld(_serviceProvider);
			// var simOpts = _simulatorMonitor.CurrentValue;
			// var renderOpts = _renderMonitor.CurrentValue;
			//
			// var basePath = Path.Combine("output", gen.ToString());
			// var brainPath = Path.Combine(basePath, "brains");
			// var framePath = Path.Combine(basePath, "frames");
			// var gifPath = Path.Combine(basePath, "run.gif");
			//
			// var renderMod = renderOpts.RenderMod ?? 1;
			// var rendering = gen % renderMod == 0;
			//
			// _logger.LogInformation("We {verb} rendering", rendering ? "are":"are not");
			//
			// foreach (var creature in world.Creatures) {
			// 	creature.Brain.CreateDotFile(brainPath, $"{creature.Id}.dot");
			// }
			//
			// var renderMachine = rendering ? new WorldRenderMachine(framePath, "w"): null;
			// var frames = new List<string>();
			//
			// for (var step = 0; step < simOpts.Steps; step++) {
			// 	world.NextTick();
			// 	if (rendering && step % renderOpts.TicksPerFrame == 0) {
			// 		frames.Add(renderMachine!.RenderWorld(world));
			// 	}
			// }
			//
			// var gifMod = renderOpts.GifRenderMod ?? 1;
			// var gifVerb = "are not";
			// if (rendering && render % gifMod == 0) {
			// 	var t = Task.Run(()=> CreateGif(frames.ToArray(), gifPath, default));
			// 	gifTasks.Add(t);
			// 	gifVerb = "are";
			// }
			//
			// _logger.LogInformation("We {verb} creating a gif", gifVerb);
		}

		await Task.WhenAll(gifTasks.ToArray());
		_applicationLifetime.StopApplication();
	}

	private async Task SolveGeneration(IServiceScope scope) {
		var solver = scope.ServiceProvider.GetRequiredService<Simulator>();
		await solver.Solve();
	}

	private void CreateContext(IServiceScope scope, int generation) {
		var contextProvider = scope.ServiceProvider.GetRequiredService<ContextProvider>();
		contextProvider.Initialize(generation);
	}

	public async Task CreateGif(string[] frames, string path, CancellationToken cancellationToken) {
		_logger.LogInformation("Creating gif with {frameCount} frames", frames.Count());
		await Giffer.CreateGif(frames, path, 7);
		_logger.LogInformation("Created gif with {frameCount} frames", frames.Count());
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation("Stopping the host");
		return Task.CompletedTask;
	}

	private void LogCurrent<T>(IOptionsMonitor<T> options) {
		var opts = options.CurrentValue;
		_logger.LogInformation("{T} Options: {@opts}", typeof(T), opts);
	}
}
