using System.ComponentModel.DataAnnotations;

namespace Common.Models.Options;

public record CreatureOptions : ConfigurationOptions {
	public const string SectionName = "Creature";

	public CreatureOptions() { }

	public CreatureOptions(int minRadius, int maxRadius, int maxSpeed, int maxViewingAngle, int maxEyesight, float oscillatorFrequency,
		float oscillatorPhaseOffset, int oscillatorAgeDivider) {
		MinRadius = minRadius;
		MaxRadius = maxRadius;
		MaxSpeed = maxSpeed;
		MaxViewingAngle = maxViewingAngle;
		MaxEyesight = maxEyesight;
		OscillatorFrequency = oscillatorFrequency;
		OscillatorPhaseOffset = oscillatorPhaseOffset;
		OscillatorAgeDivider = oscillatorAgeDivider;
	}

	[Range(2, 10)]
	public int MinRadius { get; init; } = 4;

	[Range(4, 20)]
	public int MaxRadius { get; init; } = 10;

	[Range(2, 10)]
	public int MaxSpeed { get; init; } = 4;

	[Range(10, 180)]
	public int MaxViewingAngle { get; init; } = 90;

	[Range(1, 50)]
	public int MaxEyesight { get; init; } = 5;

	public float OscillatorFrequency { get; init; } = 5000f;
	public float OscillatorPhaseOffset { get; init; } = 5000f;
	public int OscillatorAgeDivider { get; init; } = 1000;

	public override bool Validate(out ICollection<ValidationResult> results) {
		var isValid = base.Validate(out results);

		if (MinRadius > MaxRadius) {
			isValid = false;
			results.Add(new ValidationResult($"{nameof(MaxRadius)} must be higher then {nameof(MinRadius)}",
				new[] { nameof(MaxRadius), nameof(MinRadius) }));
		}

		return isValid;
	}

	public void Deconstruct(out int minRadius, out int maxRadius, out int maxSpeed, out int maxViewingAngle, out int maxEyesight,
		out float oscillatorFrequency, out float oscillatorPhaseOffset, out int oscillatorAgeDivider) {
		minRadius = MinRadius;
		maxRadius = MaxRadius;
		maxSpeed = MaxSpeed;
		maxViewingAngle = MaxViewingAngle;
		maxEyesight = MaxEyesight;
		oscillatorFrequency = OscillatorFrequency;
		oscillatorPhaseOffset = OscillatorPhaseOffset;
		oscillatorAgeDivider = OscillatorAgeDivider;
	}
}
