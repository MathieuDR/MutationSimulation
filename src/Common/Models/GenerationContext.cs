using Common.Helpers;

namespace Common.Models;

public class GenerationContext {
	public GenerationContext(string baseOutputPath, World world, int generation, Random random, bool renderFrames, bool renderGif) {
		BaseOutputPath = baseOutputPath;
		World = world;
		Generation = generation;
		Random = random;
		RenderFrames = renderFrames;
		RenderGif = renderGif;
		FileHelper.EnsurePath(BaseOutputPath);
	}

	public GenerationContext(string baseOutputPath, World world, int generation, Random random, string seed, bool renderFrames, bool renderGif) :
		this(baseOutputPath, world, generation, random, renderFrames, renderGif) => Seed = seed;


	public string BaseOutputPath { get; init; }
	public World World { get; init; }
	public int Generation { get; init; }
	public Random Random { get; init; }
	public string? Seed { get; init; }

	public bool RenderFrames { get; init; }
	public bool RenderGif { get; init; }
}
