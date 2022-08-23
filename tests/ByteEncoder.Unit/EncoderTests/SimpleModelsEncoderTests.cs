using Bogus;
using ByteEncoder.Unit.TestModels;
using Xunit.Abstractions;

namespace ByteEncoder.Unit.EncoderTests;

[UsesVerify]
public class SimpleModelsEncoderTests {
    private readonly ITestOutputHelper _helper;
    private Func<SimpleModel, byte[]> _sut = Encoder.Encode;

    public SimpleModelsEncoderTests(ITestOutputHelper helper) {
        _helper = helper;
        
        Randomizer.Seed = new Random(235);
    }
    
    [Fact]
    public Task Encode_ShouldHaveResult_WhenGivenSimpleModel() {
        //Arrange
        var model = SimpleModel.Generator.Generate();

        //Act
        var result = _sut.Invoke(model);
        
        //Assert
        return Verify(result);
    }
    
    [Fact]
    public Task Encode_ShouldHaveResult_WhenGivenSimpleModelWithNull() {
        //Arrange
        var model = SimpleModel.Generator.Generate();
        model.NullableString = null;

        //Act
        var result = _sut.Invoke(model);
        
        //Assert
        return Verify(result);
    }
}
