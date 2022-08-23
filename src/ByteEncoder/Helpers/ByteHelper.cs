using System.Text;

namespace ByteEncoder.Helpers; 

internal static class ByteHelper {
    public static byte[] GetBytes<T>(this T value) {
        return value switch {
            int concreteValue => BitConverter.GetBytes(concreteValue),
            ushort concreteValue => BitConverter.GetBytes(concreteValue),
            float concreteValue => BitConverter.GetBytes(concreteValue),
            Enum concreteValue  => BitConverter.GetBytes(Convert.ToInt32(concreteValue)),
            string concreteValue => Encoding.UTF8.GetBytes(concreteValue),
            null => Array.Empty<byte>(),
            _ => throw new ArgumentOutOfRangeException(nameof(value), value, $"Value is of type {value.GetType()}")
        };
    }

    public static IEnumerable<byte> AddBytes<T>(this IEnumerable<byte> bytes, T value) => bytes.Concat(value.GetBytes());
}
