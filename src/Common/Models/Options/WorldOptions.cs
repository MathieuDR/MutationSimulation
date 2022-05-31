using System.ComponentModel.DataAnnotations;

namespace Common.Models.Options;

public record WorldOptions : ConfigurationOptions {
	public const string SectionName = "World";

	public WorldOptions() { }

	public WorldOptions(int? worldWidth, int? worldHeight, int? worldSize, bool extraWalls, int creaturesAmount) {
		WorldWidth = worldWidth;
		WorldHeight = worldHeight;
		WorldSize = worldSize;
		ExtraWalls = extraWalls;
		CreaturesAmount = creaturesAmount;
	}

	public int Width => WorldWidth ?? WorldSize ?? 250;
	public int Height => WorldWidth ?? WorldSize ?? 250;

	public int? WorldWidth { get; init; }
	public int? WorldHeight { get; init; }
	public int? WorldSize { get; init; } = 250;
	public bool ExtraWalls { get; init; } = true;

	[Range(10, 1000)]
	public int CreaturesAmount { get; init; } = 100;


	public void Deconstruct(out int? worldWidth, out int? worldHeight, out int? worldSize, out bool extraWalls, out int creaturesAmount) {
		worldWidth = WorldWidth;
		worldHeight = WorldHeight;
		worldSize = WorldSize;
		extraWalls = ExtraWalls;
		creaturesAmount = CreaturesAmount;
	}
}
