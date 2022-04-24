using Common.Helpers;
using FluentAssertions;
using Xunit;

namespace Common.Unit.HelperTests; 

public class HexTests {
	
	
	// https://www.h-schmidt.net/FloatConverter/IEEE754.html
	[Theory]
	[InlineData(123444f, "001AF147")]
	[InlineData(0.0123f, "F085493C")]
	[InlineData(-0.0123f, "F08549BC")]
	[InlineData(999.9992f, "F3FF7944")]
	[InlineData(-999.9992f, "F3FF79C4")]
	public void ToHex_ReturnsCorrectHex_FromFloat(float value, string expected) {
		//Arrange
		//Act
		var result = value.ToHex();

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(123444f, "001AF147")]
	[InlineData(0.0123f, "F085493C")]
	[InlineData(-0.0123f, "F08549BC")]
	[InlineData(999.9992f, "F3FF7944")]
	[InlineData(-999.9992f, "F3FF79C4")]
	public void FromHex_ReturnsCorrectFloat_FromHex(float expected, string value) {
		//Arrange
		//Act
		var result = value.ToFloat();

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(123444f)]
	[InlineData(0.0123f)]
	[InlineData(-0.0123f)]
	[InlineData(999.9992f)]
	[InlineData(-999.9992f)]
	public void FromHex_ReturnsCorrectFloat_FromOriginalFloat(float original) {
		//Arrange
		var hex = original.ToHex();
		//Act
		var result = hex.ToFloat();

		//Assert
		result.Should().Be(original);
	}
}
