using Common.Models;

namespace Common.Helpers; 

public static class HexHelper {
	public static string ToHex(this float source) => BitConverter.GetBytes(source).ToHex();
	public static string ToHex(this int source) => source.ToString("X8");
	public static string ToHex(this short source) => source.ToString("X8");
	public static string ToHex(this byte source) => source.ToString("X8");
	private static string ToHex(this byte[] source) => BitConverter.ToInt32(source, 0).ToHex();

	public static string ToHex(this Neuron neuron) {
		return neuron.ToBytes().ToHex();
	}
	
	public static float FromHex(this string hex) {
		var i = Convert.ToInt32(hex, 16);
		var bytes = BitConverter.GetBytes(i);
		return BitConverter.ToSingle(bytes, 0);
	}
}
