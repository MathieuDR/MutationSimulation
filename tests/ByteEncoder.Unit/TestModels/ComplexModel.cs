using Bogus;
using ByteEncoder.Attributes;

namespace ByteEncoder.Unit.TestModels;

public record ComplexModel {
    [BytePiece(1)]
    public int MyInteger { get; set; }


    [BytePiece(2)]
    public SimpleModel? Model { get; set; }

    [BytePiece(3)]
    public List<SimpleModel>? Models { get; set; }


    public static Faker<ComplexModel> Generator => new Faker<ComplexModel>()
        .RuleFor(x => x.MyInteger, f => f.Random.Int())
        .RuleFor(x => x.Model, _ => SimpleModel.Generator.Generate())
        .RuleFor(x => x.Models, _ => SimpleModel.Generator.GenerateBetween(0, 5).ToList());
}
