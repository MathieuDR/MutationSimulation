using Common.Models.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Graphics; 

public class GifRenderer   {
	private readonly IHostApplicationLifetime _lifetime;
	private readonly ILogger<GifRenderer> _logger;
	private readonly RenderOptions _renderOptions;

	private List<Task> _tasks = new();

	public GifRenderer(IHostApplicationLifetime lifetime, ILogger<GifRenderer> logger, IOptionsSnapshot<RenderOptions> renderOptions) {
		_lifetime = lifetime;
		_logger = logger;
		_renderOptions = renderOptions.Value;
		_lifetime.ApplicationStopping.Register(() => {
			Task.WhenAll(_tasks.ToArray()).GetAwaiter().GetResult();
		});
	}

	public void StartGifRender(string[] frames) { }
}
