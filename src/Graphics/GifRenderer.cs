using Common.Models.Options;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Graphics; 

public class GifRenderer   {
	private readonly IHostApplicationLifetime _lifetime;
	private readonly ILogger<GifRenderer> _logger;
	private readonly RenderOptions _renderOptions;

	private readonly List<Task> _tasks = new();

	public GifRenderer(IHostApplicationLifetime lifetime, ILogger<GifRenderer> logger, IOptionsSnapshot<RenderOptions> renderOptions) {
		_lifetime = lifetime;
		_logger = logger;
		_renderOptions = renderOptions.Value;
		_lifetime.ApplicationStopping.Register(() => {
			_logger.LogInformation("Waiting on {0} GIF tasks to shutdown", _tasks.Count);
			Task.WhenAll(_tasks.ToArray()).GetAwaiter().GetResult();
		});
	}

	public void StartGifRender(string[] frames, string path) {
		_logger.LogTrace("Creating GIF task");
		var t = Task.Run(async () => {
			await Giffer.CreateGif(frames, path, _renderOptions.GifDelay);
		});
		_tasks.Add(t);
		t.ContinueWith(_ => {
			_tasks.RemoveAll(x => x.IsCompleted);
			_logger.LogTrace("Removed GIF task");
			_logger.LogTrace("Current GIF tasks running {0}", _tasks.Count);
		});
	}
}
