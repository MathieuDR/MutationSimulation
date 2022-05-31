using System.ComponentModel.DataAnnotations;

namespace Common.Models.Options;

public record RenderOptions : ConfigurationOptions {
	public const string SectionName = "Render";

	public RenderOptions() { }

	public RenderOptions(int? renderMod, int? gifRenderMod, bool outputAllBrain, bool outputTopBrains, int topBrainsAmount,
		string outputDirectory) {
		RenderMod = renderMod;
		GifRenderMod = gifRenderMod;
		OutputAllBrain = outputAllBrain;
		OutputTopBrains = outputTopBrains;
		TopBrainsAmount = topBrainsAmount;
		OutputDirectory = outputDirectory;
	}

	public int TicksPerFrame { get; init; } = 5;
	public int? RenderMod { get; init; } = 5;
	public int? GifRenderMod { get; init; } = 2;
	public bool OutputAllBrain { get; init; }
	public bool OutputTopBrains { get; init; } = true;
	public int TopBrainsAmount { get; init; } = 3;
	public string OutputDirectory { get; init; } = "output";
	public int PixelMultiplier { get; init; } = 1;

	public int GifDelay { get; init; } = 7;
	
	[Range(2, 8)]
	public int WallWidth { get; init; } = 4;

	public override bool Validate(out ICollection<ValidationResult> results) {
		var isValid = base.Validate(out results);

		if (WallWidth % 2 == 1) {
			isValid = false;
			results.Add(new ValidationResult($"{nameof(WallWidth)} must be even",
				new[] { nameof(WallWidth) }));
		}

		return isValid;
	}

	public void Deconstruct(out int? renderMod, out int? gifRenderMod, out bool outputAllBrain, out bool outputTopBrains, out int topBrainsAmount,
		out string outputDirectory) {
		renderMod = RenderMod;
		gifRenderMod = GifRenderMod;
		outputAllBrain = OutputAllBrain;
		outputTopBrains = OutputTopBrains;
		topBrainsAmount = TopBrainsAmount;
		outputDirectory = OutputDirectory;
	}
}
