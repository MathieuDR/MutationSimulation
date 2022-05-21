using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Main.Services; 

public class SimulationHost : IHostedService{
	private readonly ILogger<SimulationHost> _logger;
	private readonly IHostApplicationLifetime _applicationLifetime;

	public SimulationHost(ILogger<SimulationHost> logger, IHostApplicationLifetime applicationLifetime) {
		_logger = logger;
		_applicationLifetime = applicationLifetime;
	}
	
	public Task StartAsync(CancellationToken cancellationToken) {
		_logger.LogInformation("Starting the host");
		return Task.CompletedTask;
	}

	public Task StopAsync(CancellationToken cancellationToken) {
		_logger.LogInformation("Stopping the host");
		return Task.CompletedTask;
	}
}
