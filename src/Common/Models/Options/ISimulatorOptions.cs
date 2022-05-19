namespace Common.Models.Options;

public interface ISimulatorOptions {
	public int? Generations { get; }
	public int Steps { get; }
	public bool ValidateStartPositions { get; }
}
