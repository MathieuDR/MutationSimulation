using Common.Models;

namespace Common.Helpers; 

public static class HexHelper {
	public static string ToHex(this float source) {
		var @bytes = BitConverter.GetBytes(source);
		var @int = BitConverter.ToInt32(@bytes, 0);
		return @int.ToHex();
	}

	public static string ToHex(this int source) => source.ToString("X8");
	public static string ToHex(this long source) => source.ToString("X16");
	public static string ToHex(this short source) => source.ToString("X4");
	public static string ToHex(this ushort source) => source.ToString("X4");
	public static string ToHex(this byte source) => source.ToString("X1");
	
	public static float ToFloat(this string hex) {
		var i = Convert.ToInt32(hex, 16);
		var bytes = BitConverter.GetBytes(i);
		return BitConverter.ToSingle(bytes, 0);
	}

}
