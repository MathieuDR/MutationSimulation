namespace Common.Models.Genetic;

public interface IBiologicalEncodable {
	public string ToHex();
	public byte[] GetBytes();
}
