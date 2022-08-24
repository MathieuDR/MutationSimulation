using Bogus;
using ByteEncoder.Unit.TestModels;
using FluentAssertions;
using Xunit.Abstractions;

namespace ByteEncoder.Unit.EncoderTests;

[UsesVerify]
public class SimpleModelsEncoderTests {
    private readonly ITestOutputHelper _helper;
    private Func<ComplexModel, byte[]> _complexSut = Encoder.Encode;
    private Func<SimpleModel, byte[]> _simpleSut = Encoder.Encode;

    public SimpleModelsEncoderTests(ITestOutputHelper helper) {
        _helper = helper;
        
        Randomizer.Seed = new Random(235);
    }
    
    [Fact]
    public Task Encode_ShouldHaveCorrectResult_WhenGivenSimpleModel() {
        //Arrange
        var model = SimpleModel.Generator.Generate();

        //Act
        var result = _simpleSut.Invoke(model);
        
        //Assert
        return Verify(result);
    }

    
    [Fact]
    public void Encode_ShouldHaveSameResult_WhenGivenSimpleModelWithDiferentIgnoredValue() {
        //Arrange
        var model = SimpleModel.Generator.Generate();
        var firstResult = _simpleSut.Invoke(model);
        model.IgnoredInteger = 999;

        //Act
        var result = _simpleSut.Invoke(model);
        
        //Assert
        result.Should().Equal(firstResult);
    }

    [Fact]
    public Task Encode_ShouldHaveCorrectResult_WhenGivenSimpleModelWithNull() {
        //Arrange
        var model = SimpleModel.Generator.Generate();
        model.NullableString = null;

        //Act
        var result = _simpleSut.Invoke(model);
        
        //Assert
        return Verify(result);
    }
    [Fact]
    public Task Encode_ShouldGiveCorrectResult_WhenGivenComplexModel() {
        //Arrange
        var model = ComplexModel.Generator.Generate();

        //Act
        var result = _complexSut.Invoke(model);

        //Assert
        return Verify(result);
    }
}
