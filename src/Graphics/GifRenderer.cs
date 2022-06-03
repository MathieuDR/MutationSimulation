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
			Task.WhenAll(_tasks.ToArray()).GetAwaiter().GetResult();
		});
	}

	public void StartGifRender(string[] frames, string path) {
		_logger.LogInformation("Creating GIF task");
		var t = Task.Run(async () => {
			await Giffer.CreateGif(frames, path, _renderOptions.GifDelay);
		});
		_tasks.Add(t);
		t.ContinueWith(_ => {
			_tasks.RemoveAll(x => x.IsCompleted);
			_logger.LogInformation("Removed GIF task");
			_logger.LogInformation("Current tasks running {0}", _tasks.Count);
		});
	}
}
