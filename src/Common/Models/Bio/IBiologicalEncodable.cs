namespace Common.Models.Bio;

public interface IBiologicalEncodable {
	public string ToHex();
	public byte[] GetBytes();
}
