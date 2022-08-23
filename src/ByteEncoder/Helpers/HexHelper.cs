namespace ByteEncoder.Helpers; 

public static class HexHelper {
	public static string ToHex(this float source) => BitConverter.GetBytes(source).ToHex();
	public static string ToHex(this byte[] source) => Convert.ToHexString(source);

	public static float ToFloat(this string hex) {
		var bytes = Convert.FromHexString(hex);
		return BitConverter.ToSingle(bytes, 0);
	}

}
