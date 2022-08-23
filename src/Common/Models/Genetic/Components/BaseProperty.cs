using Common.Helpers;
using Common.Models.Genetic.Components.Neurons;

namespace Common.Models.Genetic.Components;



    public record Property : IBiologicalEncodable {
    
    public PropertyType PropertyType { get; init; }
    public float Value { get; init; }

    private static int ByteSize => sizeof(int) + sizeof(float); // enum = int
    

    public static Property FromHex(string hex, NeuronType externalType) {
        var bytes = Convert.FromHexString(hex);
        return FromBytes(bytes, externalType);
    }

    public static Property FromBytes(byte[] bytes, NeuronType externalType) {
        throw new NotImplementedException();
    }

    public string ToHex() => GetBytes().ToHex();

    public byte[] GetBytes() => PropertyType.GetBytes()
        .AddBytes(Value)
        .ToArray();
}
