using CommandLine;
using Common.Models.Enums;
using Common.Models.Options;

namespace Main.Models;

public class Options : ISimulatorOptions, IWorldOptions, IBrainOptions, IRenderOptions, IRandomOptions, ICreatureOptions {
	public Options(int startNeurons, int maxInternalNeuron, ActivationFunction internalActivationFunction, int minRadius, int maxRadius, int maxSpeed, int maxViewingAngle, int maxEyesight, float oscillatorFrequency, float oscillatorPhaseOffset, int oscillatorAgeDivider, string seed, bool useSeed, int? renderMod, int? gifRenderMod, bool outputAllBrain, bool outputTopBrains, int topBrainsAmount, string outputDirectory, int? generations, int steps, bool validateStartPositions, int? worldWidth, int? worldHeight, int? worldSize, bool extraWalls, int wallWidth, int creaturesAmount) {
		StartNeurons = startNeurons;
		MaxInternalNeuron = maxInternalNeuron;
		InternalActivationFunction = internalActivationFunction;
		MinRadius = minRadius;
		MaxRadius = maxRadius;
		MaxSpeed = maxSpeed;
		MaxViewingAngle = maxViewingAngle;
		MaxEyesight = maxEyesight;
		OscillatorFrequency = oscillatorFrequency;
		OscillatorPhaseOffset = oscillatorPhaseOffset;
		OscillatorAgeDivider = oscillatorAgeDivider;
		Seed = seed;
		UseSeed = useSeed;
		RenderMod = renderMod;
		GifRenderMod = gifRenderMod;
		OutputAllBrain = outputAllBrain;
		OutputTopBrains = outputTopBrains;
		TopBrainsAmount = topBrainsAmount;
		OutputDirectory = outputDirectory;
		Generations = generations;
		Steps = steps;
		ValidateStartPositions = validateStartPositions;
		WorldWidth = worldWidth;
		WorldHeight = worldHeight;
		WorldSize = worldSize;
		ExtraWalls = extraWalls;
		WallWidth = wallWidth;
		CreaturesAmount = creaturesAmount;
	}

	// Brain options
	[Option("initial-neurons", Default = 20)]
	public int StartNeurons { get; }
	[Option("max-neurons", Default = 20)]
	public int MaxInternalNeuron { get; }

	[Option("activation", Default = ActivationFunction.ReLu)]
	public ActivationFunction InternalActivationFunction { get; }

	// Creature options
	[Option("min-radius", Default = 4)]
	public int MinRadius { get; }
	[Option("max-radius", Default = 10)]
	public int MaxRadius { get; }
	[Option("max-speed", Default = 4)]
	public int MaxSpeed { get; }
	[Option("fov", Default = 90)]
	public int MaxViewingAngle { get; }
	[Option("max-eyesight", Default = 5)]
	public int MaxEyesight { get; }
	[Option("osc-freq", Default = 5000f)]
	public float OscillatorFrequency { get; }
	
	[Option("osc-phase", Default = 5000f)]
	public float OscillatorPhaseOffset { get; }
	[Option("osc-divider", Default = 1000)]
	public int OscillatorAgeDivider { get; }

	// Misc
	[Option('r',"seed", Default = "default")]
	public string Seed { get; }
	[Option("use-seed", Default = true)]
	public bool UseSeed { get; }

	// render options
	[Option('m',"render-modulo", Default = 5)]
	public int? RenderMod { get; }
	[Option('g',"gif-render-modulo", Default = 1)]
	public int? GifRenderMod { get; }
	[Option('b',"output-all-brains", Default = false)]
	public bool OutputAllBrain { get; }
	[Option("output-top-brains", Default = true)]
	public bool OutputTopBrains { get; }
	[Option("output-top-brains-amount", Default = 3)]
	public int TopBrainsAmount { get; }
	[Option('o',"output", Default = "output")]
	public string OutputDirectory { get; }

	// Simulation options
	[Option('g',"generations", Default = 5000)]
	public int? Generations { get; }
	[Option('a',"max-age", Default = 500)]
	public int Steps { get; }
	[Option("validate-start", Default = true)]
	public bool ValidateStartPositions { get; }

	// World options
	[Option('w',"width", Default = null, SetName = "NonUniformSize")]
	public int? WorldWidth { get; }

	[Option('h',"height", Default = null, SetName = "NonUniformSize")]
	public int? WorldHeight { get; }

	[Option('s', "Size", Default = 250, SetName = "UniformSize")]
	public int? WorldSize { get; }

	[Option("walls", Default = true)]
	public bool ExtraWalls { get; }
	[Option("wall-width", Default = 4)]
	public int WallWidth { get; }
	[Option('c',"creatures", Default = 100)]
	public int CreaturesAmount { get; }
}
