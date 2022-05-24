namespace Common.Models.Options;

public record RandomOptions : ConfigurationOptions {
	public const string SectionName = "Random";

	public RandomOptions() { }

	public RandomOptions(string seed, bool useSeed) {
		Seed = seed;
		UseSeed = useSeed;
	}

	public string Seed { get; init; } = "default";
	public bool UseSeed { get; init; } = true;

	public void Deconstruct(out string seed, out bool useSeed) {
		seed = Seed;
		useSeed = UseSeed;
	}
}
