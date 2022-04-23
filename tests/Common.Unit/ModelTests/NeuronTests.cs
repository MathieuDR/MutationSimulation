using System;
using Common.Helpers;
using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class NeuronTests {
	[Fact]
	public void Constructor_ShouldThrowsError_WhenIdIsOver128() {
		//Arrange;
		//Act
		Action act = () => {
			_ = new Neuron(129, NeuronType.Input);
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
	public void ConstructorWith_ShouldThrowsError_WhenIdIsOver128() {
		//Arrange;
		var neuron = new Neuron(5, NeuronType.Internal);
		//Act
		Action act = () => {
			neuron = neuron with { Id = 129 };
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
		var b = neuron.ToByte();

		//Assert
		b.Should().Be(0b10000001);
	}
	
	[Fact]
	public void ToBytes_ShouldHaveSetLeftMostBit_ForOutputNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Output);

		//Act
		var b = neuron.ToByte();

		//Assert
		b.Should().Be(0b10000001);
	}
	
	[Fact]
	public void ToBytes_ShouldHaveUnsetLeftMostBit_ForInternalNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Internal);

		//Act
		var b = neuron.ToByte();

		//Assert
		b.Should().Be(0b00000001);
	}
	
	[Theory]
	[InlineData(0x7F, 0b01111111)]
	[InlineData(0x70, 0b01110000)]
	public void ToBytes_ShouldHaveCorrectByteValue_ForValidId(byte id, byte expected) {
		//Arrange
		var neuron = new Neuron(id, NeuronType.Internal);

		//Act
		var b = neuron.ToByte();

		//Assert
		b.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0x7F, NeuronType.Internal, "7F")]
	[InlineData(0x70, NeuronType.Internal, "70")]
	[InlineData(0x79, NeuronType.Input, "F9")]
	[InlineData(0x79, NeuronType.Output, "F9")]
	public void ToString_ShouldHaveCorrectHexValue_ForValidParams(byte id, NeuronType type, string expected) {
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
	[InlineData(0x79, NeuronType.Input, NeuronType.Input,"F9")]
	[InlineData(0x79, NeuronType.Output, NeuronType.Output,"F9")]
	public void FromHex_ShouldHaveCorrectNeuron_ForValidHex(byte id, NeuronType type,NeuronType externalType ,string hex) {
		//Arrange
		

		//Act
		var neuron = hex.FromHex(externalType);

		//Assert
		neuron.Id.Should().Be(id);
		neuron.NeuronType.Should().Be(type);
	}
	
	
}
