using System.Text;

namespace ByteEncoder.Helpers;

internal static class ByteHelper {
    public static byte[] GetBytes(this object? value, Type t) {
        if (value is null) {
            return Array.Empty<byte>();
        }

        if (t.IsPrimitive) {
            return GetBytesForPrimitive(value);
        }

        return GetBytesForNonPrimitive(value);
    }

    public static byte[] GetBytes<T>(this T value) => GetBytes(value, value?.GetType() ?? typeof(object));

    private static byte[] GetBytesForNonPrimitive<T>(this T value) =>
        value switch {
            string concreteValue => Encoding.UTF8.GetBytes(concreteValue),
            Enum concreteValue => BitConverter.GetBytes(Convert.ToInt32(concreteValue)),
            null => Array.Empty<byte>(),
            _ => Encoder.Encode(value)
        };

    private static byte[] GetBytesForPrimitive<T>(this T value)
        =>
            value switch {
                int concreteValue => BitConverter.GetBytes(concreteValue),
                ushort concreteValue => BitConverter.GetBytes(concreteValue),
                float concreteValue => BitConverter.GetBytes(concreteValue),
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, $"Value is of type {value.GetType()}")
            };


    public static IEnumerable<byte> AddBytes<T>(this IEnumerable<byte> bytes, T value) => bytes.Concat(value.GetBytes());
    public static IEnumerable<byte> AddBytes(this IEnumerable<byte> bytes, object? value, Type t) => bytes.Concat(value.GetBytes(t));
}
