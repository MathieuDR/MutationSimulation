using Common.Models;

namespace Common.FitnessTests.Parts; 

public interface IFitnessPart {
	public double Score(Creature creature);
}