using CommandLine;

namespace Main.Models; 

public record CmdArgs {
	[Option('e', "environment")]
	public string? Environment { get; init; }
}
