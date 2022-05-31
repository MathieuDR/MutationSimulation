using Common.Factories;
using Common.Helpers;
using Common.Models;
using Common.Models.Options;
using Common.Services;
using Microsoft.Extensions.Options;

namespace Main.Services; 

public class ContextProvider {
	private readonly WorldFactory _worldFactory;
	private readonly IRandomProvider _randomProvider;
	private readonly RenderOptions _renderOptions;
	public GenerationContext? Context { get; private set; } = null;

	public ContextProvider(WorldFactory worldFactory, IOptionsSnapshot<RenderOptions> renderOptions, IRandomProvider randomProvider) {
		_worldFactory = worldFactory;
		_randomProvider = randomProvider;
		_renderOptions = renderOptions.Value;
	}

	public void Initialize(int generation) {
		var world = _worldFactory.Create();
		var render = CalculateRenders(generation, out var renderGif);
		var outputDir = CreateOutputPath(generation);
		
		Context = new GenerationContext(outputDir, world, generation, _randomProvider.GetRandom(), render, renderGif) {
			Seed = _randomProvider.SeedString
		};
	}

	private bool CalculateRenders(int generation, out bool renderGif) {
		var render = generation % _renderOptions.RenderMod == 0;
		var rendered = generation / _renderOptions.RenderMod;
		renderGif = render && rendered % _renderOptions.GifRenderMod == 0;
		return render;
	}

	private string CreateOutputPath(int generation) {
		var outputDir = string.IsNullOrWhiteSpace(_renderOptions.OutputDirectory) ? "output" : _renderOptions.OutputDirectory;
		var genPath = generation.ToString("0000");
		var path = Path.Combine(outputDir, genPath);
		FileHelper.EnsurePath(path);
		return path;
	}
}
