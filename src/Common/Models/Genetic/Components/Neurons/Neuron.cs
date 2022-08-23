using Common.Helpers;

namespace Common.Models.Genetic.Components.Neurons;

public record Neuron {
    private const int TypeMask = 1 << 7;
    private const int IdAndMask = byte.MaxValue - TypeMask;
    public const int ByteSize = sizeof(ushort) + sizeof(float);
    private const float BiasDivider = float.MaxValue;
    public const int MaxId = ushort.MaxValue / 2;
    private readonly ushort _id;
    private readonly float _bias;

    public float Bias {
        get => _bias;
        init {
            var temp = value / BiasDivider;
            if (temp is <= -1f or >= 1f) {
                throw new ArgumentOutOfRangeException(nameof(value), "Bias must be in range [-1, 1]");
            }

            _bias = temp;
        }
    }

    public Neuron(ushort Id, float Bias, NeuronType NeuronType) {
        this.Id = Id;
        this.NeuronType = NeuronType;
        this.Bias = Bias;
    }

    public ushort Id {
        get => _id;
        init {
            if (value > MaxId) {
                throw new ArgumentOutOfRangeException(nameof(Id), $"Id must be higher than {MaxId}");
            }

            _id = value;
        }
    }

    public NeuronType NeuronType { get; init; }

    public static float BiasToFloat(float bias) {
        if (bias is < -1f or > 1f) {
            throw new ArgumentOutOfRangeException(nameof(bias), "Bias must be in range [-1, 1]");
        }

        return bias * BiasDivider;
    }

    public Neuron ToMemoryNeuron() => this with { NeuronType = NeuronType.Memory };

    public void Deconstruct(out ushort Id, out float Bias, out NeuronType NeuronType) {
        Id = this.Id;
        NeuronType = this.NeuronType;
        Bias = BiasToFloat(this.Bias);
    }

    public string ToHex() => GetBytes().ToHex();

    public byte[] GetBytes() {
        var idBytes = BitConverter.GetBytes(Id);

        // If it's an internal neuron, the leftmost bit is unset
        if (NeuronType != NeuronType.Internal) {
            idBytes[^1] |= TypeMask;
        }

        return idBytes
            .Concat(BitConverter.GetBytes(BiasToFloat(Bias)))
            .ToArray();
    }

    public static Neuron FromBytes(byte[] bytes, NeuronType externalType) {
        var type = (bytes[^1] & TypeMask) != 0 ? externalType : NeuronType.Internal;
        bytes[^1] &= IdAndMask;
        var id = BitConverter.ToUInt16(bytes.Take(sizeof(ushort)).ToArray(), 0);

        var bias = BitConverter.ToSingle(bytes.Skip(sizeof(ushort)).Take(sizeof(float)).ToArray());

        return type switch {
            NeuronType.Input => new InputNeuron(id, bias),
            NeuronType.Action => new ActionNeuron(id, bias),
            NeuronType.Internal => new Neuron(id, bias, NeuronType.Internal),
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public static Neuron FromHex(string hex, NeuronType externalType) {
        var bytes = Convert.FromHexString(hex);
        return FromBytes(bytes, externalType);
    }
}
