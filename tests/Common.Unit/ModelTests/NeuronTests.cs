using System;
using Common.Helpers;
using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class NeuronTests {
	[Fact]
	public void Constructor_ShouldThrowsError_WhenIdIsOver32768() {
		//Arrange;
		//Act
		Action act = () => {
			_ = new Neuron(32768, NeuronType.Input);
		};
		
		//Assert
		act.Should().Throw<ArgumentOutOfRangeException>();
	}
	
	[Fact]
	public void Constructor_ShouldThrowsError_WhenIdIs0() {
		//Arrange;
		//Act
		Action act = () => {
			_ = new Neuron(0, NeuronType.Input);
		};
		
		//Assert
		act.Should().Throw<ArgumentOutOfRangeException>();
	}
	
	[Fact]
	public void ConstructorWith_ShouldThrowsError_WhenIdIsOver32768() {
		//Arrange;
		var neuron = new Neuron(5, NeuronType.Internal);
		//Act
		Action act = () => {
			neuron = neuron with { Id = 32768 };
		};
		
		//Assert
		act.Should().Throw<ArgumentOutOfRangeException>();
	}
	
	[Fact]
	public void Constructor_ShouldThrowsNoError_WhenIdIsSmallerThan128() {
		//Arrange;
		//Act
		Action act = () => {
			_ = new Neuron(127, NeuronType.Input);
		};
		
		//Assert
		act.Should().NotThrow();
	}
	
	[Fact]
	public void ToBytes_ShouldHaveSetLeftMostBit_ForInputNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Input);

		//Act
		var b = neuron.ToBytes()[^1];

		//Assert
		b.Should().Be(0b10000000);
	}
	
	[Fact]
	public void ToBytes_ShouldHaveSetLeftMostBit_ForOutputNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Output);

		//Act
		var b = neuron.ToBytes()[^1];

		//Assert
		b.Should().Be(0b10000000);
	}
	
	[Fact]
	public void ToBytes_ShouldHaveUnsetLeftMostBit_ForInternalNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Internal);

		//Act
		var b = neuron.ToBytes()[^1];

		//Assert
		b.Should().Be(0b00000000);
	}
	
	[Theory]
	[InlineData(0x7F, 0x7F)]
	[InlineData(0x70, 0x70)]
	[InlineData(32767, 32767)]
	public void ToBytes_ShouldHaveCorrectByteValue_WhenNeuronIsInternal(ushort id, ushort expected) {
		//Arrange
		var neuron = new Neuron(id, NeuronType.Internal);

		//Act
		var b = neuron.ToBytes();
		var actual = BitConverter.ToUInt16(b, 0);

		//Assert
		actual.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0x7F, 0x7F+32768)]
	[InlineData(0x70, 0x70+32768)]
	[InlineData(32767, 32767+32768)]
	public void ToBytes_ShouldHaveCorrectByteValue_WhenNeuronIsExternal(ushort id, ushort expected) {
		//Arrange
		var neuron = new Neuron(id, NeuronType.Output);

		//Act
		var b = neuron.ToBytes();
		var actual = BitConverter.ToUInt16(b, 0);

		//Assert
		actual.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0x7F, NeuronType.Internal, "007F")]
	[InlineData(0x70, NeuronType.Internal, "0070")]
	[InlineData(0x79, NeuronType.Input, "8079")]
	[InlineData(0x79, NeuronType.Output, "8079")]
	public void ToString_ShouldHaveCorrectHexValue_ForValidParams(ushort id, NeuronType type, string expected) {
		//Arrange
		var neuron = new Neuron(id, type);

		//Act
		var b = neuron.ToString();

		//Assert
		b.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0x7F, NeuronType.Internal, NeuronType.Input, "7F")]
	[InlineData(0x70, NeuronType.Internal, NeuronType.Input,"70")]
	[InlineData(0x79, NeuronType.Input, NeuronType.Input,"8079")]
	[InlineData(0x79, NeuronType.Output, NeuronType.Output,"8079")]
	public void FromHex_ShouldHaveCorrectNeuron_ForValidHex(ushort id, NeuronType type,NeuronType externalType ,string hex) {
		//Arrange
		

		//Act
		var neuron = hex.FromHex(externalType);

		//Assert
		neuron.Id.Should().Be(id);
		neuron.NeuronType.Should().Be(type);
	}
	
	
}
