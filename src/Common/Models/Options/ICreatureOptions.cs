namespace Common.Models.Options;

public interface ICreatureOptions {
	public int MinRadius { get; }
	public int MaxRadius { get; }
	public int MaxSpeed { get; }
	public int MaxViewingAngle { get; }
	public int MaxEyesight { get; }
	public float OscillatorFrequency { get; }
	public float OscillatorPhaseOffset { get; }
	public int OscillatorAgeDivider { get; }
}
