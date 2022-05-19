namespace Common.Models.Options;

public interface IWorldOptions {
	public int? WorldWidth { get; }
	public int? WorldHeight { get; }
	public int? WorldSize { get; }
	public bool ExtraWalls { get; }
	public int WallWidth { get; }
	public int CreaturesAmount { get; }
}
