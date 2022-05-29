using CommandLine;

namespace Main.Models; 

public record CmdArgs {
	public CmdArgs() {
		
	}
	public CmdArgs(string? environment) => Environment = environment;

	[Option('e', "environment")]
	public string? Environment { get; init; }
}
