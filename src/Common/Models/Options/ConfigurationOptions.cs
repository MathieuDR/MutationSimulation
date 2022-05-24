using System.ComponentModel.DataAnnotations;

namespace Common.Models.Options;

public abstract record ConfigurationOptions {
	public virtual bool Validate(out ICollection<ValidationResult> results) => ValidateDataAnnotations(out results);

	private bool ValidateDataAnnotations(out ICollection<ValidationResult> results) {
		results = new List<ValidationResult>();
		return Validator.TryValidateObject(this, new ValidationContext(this), results, true);
	}
}
