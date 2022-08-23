using Bogus;
using ByteEncoder.Attributes;

namespace ByteEncoder.Unit.TestModels; 

public record SimpleModel {
    [BytePiece(1)]
    public int MyInteger { get; set; }
    public int IgnoredInteger { get; set; }
    [BytePiece(2)]
    public string StringValue { get; set; } = "NotNullable";
    [BytePiece(40)]
    public float FloatValue { get; set; }
    [BytePiece(20)]
    public string? NullableString { get; set; }

    [BytePiece(5)]
    public int MyField;
    [BytePiece(0)]
    public float OtherField;

    public static Faker<SimpleModel> Generator => new Faker<SimpleModel>()
        .RuleFor(x => x.MyInteger, f => f.Random.Int())
        .RuleFor(x => x.IgnoredInteger, f => f.Random.Int())
        .RuleFor(x => x.MyField, f => f.Random.Int())
        .RuleFor(x => x.FloatValue, f => f.Random.Float())
        .RuleFor(x => x.OtherField, f => f.Random.Float())
        .RuleFor(x => x.StringValue, f => f.Random.Word())
        .RuleFor(x => x.NullableString, f => f.Random.Word().OrNull(f, 0.3f));
}
