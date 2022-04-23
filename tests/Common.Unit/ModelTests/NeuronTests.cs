using System;
using System.Linq;
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
	public void ToBytes_ShouldOnlyHave1Byte_WhenProvidedValidNeuron() {
		//Arrange
		var neuron = new Neuron(50, NeuronType.Input);

		//Act
		var bytes = neuron.ToBytes();

		//Assert
		bytes.Should().HaveCount(1);
	}

	[Fact]
	public void ToBytes_ShouldHaveSetLeftMostBit_ForInputNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Input);

		//Act
		var b = neuron.ToBytes().First();

		//Assert
		b.Should().Be(0b10000001);
	}
	
	[Fact]
	public void ToBytes_ShouldHaveSetLeftMostBit_ForOutputNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Output);

		//Act
		var b = neuron.ToBytes().First();

		//Assert
		b.Should().Be(0b10000001);
	}
	
	[Fact]
	public void ToBytes_ShouldHaveUnsetLeftMostBit_ForInternalNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Internal);

		//Act
		var b = neuron.ToBytes().First();

		//Assert
		b.Should().Be(0b00000001);
	}
	
	[Theory]
	[InlineData(0x7F, 0b01111111)]
	[InlineData(0x70, 0b01110000)]
	public void ToBytes_ShouldHaveCorrectHexValue_ForValidId(byte id, byte expected) {
		//Arrange
		var neuron = new Neuron(id, NeuronType.Internal);

		//Act
		var b = neuron.ToBytes().First();

		//Assert
		b.Should().Be(expected);
	}
}
