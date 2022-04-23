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
	
	[Theory]
	[InlineData(0xFF, "00000000000000FF")]
	[InlineData(0x10, "0000000000000010")]
	[InlineData(0xA0A, "0000000000000A0A")]
	[InlineData(0x12345ABF, "0000000012345ABF")]
	[InlineData(-5, "FFFFFFFFFFFFFFFB")]
	[InlineData(-523123, "FFFFFFFFFFF8048D")]
	[InlineData(long.MaxValue, "7FFFFFFFFFFFFFFF")]
	public void ToHex_ReturnsCorrectHex_FromLong(long value, string expected) {
		//Arrange
		//Act
		var result = value.ToHex();

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0xF, "F")]
	[InlineData(0x0, "0")]
	[InlineData(0xA, "A")]
	[InlineData(0x4, "4")]
	[InlineData(0xFF, "FF")]
	[InlineData(0x20, "20")]
	[InlineData(0xA0, "A0")]
	[InlineData(0x49, "49")]
	public void ToHex_ReturnsCorrectHex_FromByte(byte value, string expected) {
		//Arrange
		//Act
		var result = value.ToHex();

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0xFF, "00FF")]
	[InlineData(0x10, "0010")]
	[InlineData(0xA0A, "0A0A")]
	[InlineData(0x5ABF, "5ABF")]
	[InlineData(-5, "FFFB")]
	[InlineData(-523, "FDF5")]
	public void ToHex_ReturnsCorrectHex_FromShort(short value, string expected) {
		//Arrange
		//Act
		var result = value.ToHex();

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0xFF, "00FF")]
	[InlineData(0x10, "0010")]
	[InlineData(0xA0A, "0A0A")]
	[InlineData(0x5ABF, "5ABF")]
	[InlineData(32768, "8000")]
	[InlineData(65535, "FFFF")]
	public void ToHex_ReturnsCorrectHex_FromUshort(ushort value, string expected) {
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
		var result = value.ToFloat();

		//Assert
		result.Should().Be(expected);
	}
}
