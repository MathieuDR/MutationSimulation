using Common.Factories;
using Common.Helpers;
using Common.Models;
using Common.Models.Options;
using Common.Services;
using Microsoft.Extensions.Options;

namespace Main.Services; 

public class ContextProvider {
	private readonly WorldFactory _worldFactory;
	private readonly RandomOptions _randomOptions;
	private readonly IRandomProvider _randomProvider;
	private readonly RenderOptions _renderOptions;
	public GenerationContext? Context { get; private set; } = null;

	public ContextProvider(WorldFactory worldFactory, IOptionsSnapshot<RenderOptions> renderOptions, IOptionsSnapshot<RandomOptions> randomOptions, IRandomProvider randomProvider) {
		_worldFactory = worldFactory;
		_randomOptions = randomOptions.Value;
		_randomProvider = randomProvider;
		_renderOptions = renderOptions.Value;
	}

	public void Initialize(int generation, CancellationToken cancellationToken) {
		var world = _worldFactory.Create();
		var shouldRender = CalculateRenderFlags(generation, out var shouldRenderGif);
		var outputDir = CreateOutputPath(generation);
		
		// Initialize random
		if (!_randomOptions.UseSeed) {
			return;
		}

		var seed = $"{_randomOptions.Seed}_{generation:D4}";
		_randomProvider.SetSeed(seed);
		
		Context = new GenerationContext(outputDir, world, generation, _randomProvider.GetRandom(), shouldRender, shouldRenderGif) {
			Seed = seed
		};
	}

	private bool CalculateRenderFlags(int generation, out bool renderGif) {
		var render = generation % _renderOptions.RenderMod == 0;
		var rendered = generation / _renderOptions.RenderMod;
		renderGif = render && rendered % _renderOptions.GifRenderMod == 0;
		return render;
	}

	private string CreateOutputPath(int generation) {
		var outputDir = string.IsNullOrWhiteSpace(_renderOptions.OutputDirectory) ? "output" : _renderOptions.OutputDirectory;
		var genPath = generation.ToString("D4");
		var path = Path.Combine(outputDir, genPath);
		FileHelper.EnsurePath(path);
		return path;
	}
}
