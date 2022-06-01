using System.ComponentModel.DataAnnotations;

namespace Common.Models.Options; 

public record FitnessOptions :ConfigurationOptions {
	public const string SectionName = "Fitness";

	public FitnessOptions() {
		
	}
	public FitnessOptions(FitnessChainPart[] chain) => Chain = chain;
	public FitnessChainPart[] Chain { get; init; }

	public override bool Validate(out ICollection<ValidationResult> results) {
		var isValid = base.Validate(out results);

		if (Chain.Length == 0) {
			isValid = false;
			results.Add(new ValidationResult($"{nameof(Chain)} must contain at least one part",
				new[] { nameof(Chain) }));
		}

		return isValid;
	}
}