using ByteEncoder.Attributes;
using ByteEncoder.Helpers;

namespace ByteEncoder;

internal record MemberInfo<T>(string Name, T Value);

internal class Encoder {
    public static byte[] Encode<T>(T toEncode) {
        List<(string Name, Type Type, object? Value, BytePiece Attribute)> members = GetMembersWithValues(toEncode);


        IEnumerable<byte> result = Array.Empty<byte>();
        foreach (var member in members) {
            result = result.AddBytes(member.Value);
        }

        return result.ToArray();
    }

    private static List<(string Name, Type Type, object? Value, BytePiece Attribute)> GetMembersWithValues<T>(T toEncode) {
        var t = typeof(T);

        var fields = t.GetFields()
            .Where(x => Attribute.IsDefined(x, typeof(BytePiece)))
            .Select(x => (Name: x.Name, Type: x.FieldType, Value: x.GetValue(toEncode),
                Attribute: (BytePiece)x.GetCustomAttributes(typeof(BytePiece), false).First()));

        var props = t.GetProperties()
            .Where(x => Attribute.IsDefined(x, typeof(BytePiece)))
            .Select(x => (Name: x.Name, Type: x.PropertyType, Value: x.GetValue(toEncode),
                Attribute: (BytePiece)x.GetCustomAttributes(typeof(BytePiece), false).First() ));

        var members = fields.Concat(props)
            .Where(x=> x.Value is not null)
            .OrderBy(x => x.Attribute.Order ?? 99999)
            .ThenBy(x => x.Name)
            .ToList();
        
        return members;
    }
}
