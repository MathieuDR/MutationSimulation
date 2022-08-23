using ByteEncoder.Helpers;

namespace ByteEncoder; 

public static class ByteEncoder {
    public static byte[] Encode<T>(T toEncode) => Encoder.Encode(toEncode);

    public static string EncodeToHex<T>(T toEncode) => Encode(toEncode).ToHex();
    public static T Decode<T>(byte[] bytes) {
        throw new NotImplementedException();
    }

    public static T Decode<T>(string hex) => Decode<T>(Convert.FromHexString(hex));
}
