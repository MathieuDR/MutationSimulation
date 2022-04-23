using Common.Models;

namespace Common.Helpers; 

public static class HexHelper {
	public static string ToHex(this float source) {
		var @bytes = BitConverter.GetBytes(source);
		var @int = BitConverter.ToInt32(@bytes, 0);
		return @int.ToHex();
	}

	public static string ToHex(this int source) => source.ToString("X8");
	public static string ToHex(this short source) => source.ToString("X4");
	public static string ToHex(this byte source) => source.ToString("X1");
	public static string ToHex(this Neuron neuron) {
		return neuron.ToByte().ToHex();
	}
	
	public static float FromHex(this string hex) {
		var i = Convert.ToInt32(hex, 16);
		var bytes = BitConverter.GetBytes(i);
		return BitConverter.ToSingle(bytes, 0);
	}

	public static Neuron FromHex(this string hex, NeuronType externalType) {
		var i = Convert.ToByte(hex, 16);
		return Neuron.FromByte(i, externalType);
	}

}
