using CommandLine;

namespace Main.Models; 

public record CmdArgs {
	public CmdArgs() {
		
	}

	public CmdArgs(string? environment, bool renderGui) {
		Environment = environment;
		RenderGui = renderGui;
	}

	[Option('e', "environment")]
	public string? Environment { get; init; }
	
	
	[Option('r', "render-gui", Default = false)]
	public bool RenderGui { get; init; }
}
