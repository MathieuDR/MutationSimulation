using System.ComponentModel.DataAnnotations;

namespace Common.Models.Options;

public record WorldOptions : ConfigurationOptions {
	public const string SectionName = "World";

	public WorldOptions() { }

	public WorldOptions(int? worldWidth, int? worldHeight, int? worldSize, bool extraWalls, int wallWidth, int creaturesAmount) {
		WorldWidth = worldWidth;
		WorldHeight = worldHeight;
		WorldSize = worldSize;
		ExtraWalls = extraWalls;
		WallWidth = wallWidth;
		CreaturesAmount = creaturesAmount;
	}

	public int Width => WorldWidth ?? WorldSize ?? 250;
	public int Height => WorldWidth ?? WorldSize ?? 250;

	public int? WorldWidth { get; init; }
	public int? WorldHeight { get; init; }
	public int? WorldSize { get; init; } = 250;
	public bool ExtraWalls { get; init; } = true;

	[Range(2, 8)]
	public int WallWidth { get; init; } = 4;

	[Range(10, 1000)]
	public int CreaturesAmount { get; init; } = 100;

	public override bool Validate(out ICollection<ValidationResult> results) {
		var isValid = base.Validate(out results);

		if (WallWidth % 2 == 1) {
			isValid = false;
			results.Add(new ValidationResult($"{nameof(WallWidth)} must be even",
				new[] { nameof(WallWidth) }));
		}

		return isValid;
	}

	public void Deconstruct(out int? worldWidth, out int? worldHeight, out int? worldSize, out bool extraWalls, out int wallWidth,
		out int creaturesAmount) {
		worldWidth = WorldWidth;
		worldHeight = WorldHeight;
		worldSize = WorldSize;
		extraWalls = ExtraWalls;
		wallWidth = WallWidth;
		creaturesAmount = CreaturesAmount;
	}
}
