namespace Common.Models.Options;

public interface IRenderOptions {
	public int? RenderMod { get; }
	public int? GifRenderMod { get; }
	public bool OutputAllBrain { get; }
	public bool OutputTopBrains { get; }
	public int TopBrainsAmount { get; }
	public string OutputDirectory { get; }
}
