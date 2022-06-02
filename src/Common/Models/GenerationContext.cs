using Common.Helpers;

namespace Common.Models;

public class GenerationContext {
	public GenerationContext(string baseOutputPath, World world, int generation, Random random, bool renderFrames, bool renderGif, CancellationToken token) {
		BaseOutputPath = baseOutputPath;
		World = world;
		Generation = generation;
		Random = random;
		RenderFrames = renderFrames;
		RenderGif = renderGif;
		CancellationToken = token;
	}

	public GenerationContext(string baseOutputPath, World world, int generation, Random random, string seed, bool renderFrames, bool renderGif, CancellationToken token) :
		this(baseOutputPath, world, generation, random, renderFrames, renderGif,token) => Seed = seed;



	public void NextTick() => Tick++;
	
	public int Tick { get; private set; }
	
	CancellationToken CancellationToken { get; init; }
	public string BaseOutputPath { get; init; }
	public World World { get; init; }
	public Creature[] Creatures => World.Creatures;
	public int Generation { get; init; }
	public Random Random { get; init; }
	public string? Seed { get; init; }
	public bool RenderFrames { get; init; }
	public bool RenderGif { get; init; }
}
