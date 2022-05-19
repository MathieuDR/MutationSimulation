using Common.Models.Enums;

namespace Common.Models.Options;

public interface IBrainOptions {
	public int StartNeurons { get; }
	public int MaxInternalNeuron { get; }
	public ActivationFunction InternalActivationFunction { get; }
}
