using Common.Helpers;
using FluentAssertions;
using Xunit;

namespace Common.Unit.HelperTests; 

public class HexTests {
	[Theory]
	[InlineData(0xFF, "000000FF")]
	[InlineData(0x10, "00000010")]
	[InlineData(0xA0A, "00000A0A")]
	[InlineData(0x12345ABF, "12345ABF")]
	[InlineData(-5, "FFFFFFFB")]
	[InlineData(-523123, "FFF8048D")]
	public void ToHex_ReturnsCorrectHex_FromInt(int value, string expected) {
		//Arrange
		//Act
		var result = value.ToHex();

		//Assert
		result.Should().Be(expected);
	}
	
	// https://www.h-schmidt.net/FloatConverter/IEEE754.html
	[Theory]
	[InlineData(123444f, "47F11A00")]
	[InlineData(0.0123f, "3C4985F0")]
	[InlineData(-0.0123f, "BC4985F0")]
	[InlineData(999.9992f, "4479FFF3")]
	[InlineData(-999.9992f, "C479FFF3")]
	public void ToHex_ReturnsCorrectHex_FromFloat(float value, string expected) {
		//Arrange
		//Act
		var result = value.ToHex();

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(123444f, "47F11A00")]
	[InlineData(0.0123f, "3C4985F0")]
	[InlineData(-0.0123f, "BC4985F0")]
	[InlineData(999.9992f, "4479FFF3")]
	[InlineData(-999.9992f, "C479FFF3")]
	public void FromHex_ReturnsCorrectFloat_FromHex(float expected, string value) {
		//Arrange
		//Act
		var result = value.FromHex();

		//Assert
		result.Should().Be(expected);
	}
}
