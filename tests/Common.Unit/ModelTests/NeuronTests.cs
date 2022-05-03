using System;
using Common.Helpers;
using Common.Models;
using Common.Models.Bio;
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
		var b = neuron.GetBytes()[^1];

		//Assert
		b.Should().Be(0b10000000);
	}
	
	[Fact]
	public void ToBytes_ShouldHaveSetLeftMostBit_ForOutputNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Action);

		//Act
		var b = neuron.GetBytes()[^1];

		//Assert
		b.Should().Be(0b10000000);
	}
	
	[Fact]
	public void ToBytes_ShouldHaveUnsetLeftMostBit_ForInternalNeuronType() {
		//Arrange
		var neuron = new Neuron(1, NeuronType.Internal);

		//Act
		var b = neuron.GetBytes()[^1];

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
		var b = neuron.GetBytes();
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
		var neuron = new Neuron(id, NeuronType.Action);

		//Act
		var b = neuron.GetBytes();
		var actual = BitConverter.ToUInt16(b, 0);

		//Assert
		actual.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0x7F, NeuronType.Internal, "7F00")]
	[InlineData(0x70, NeuronType.Internal, "7000")]
	[InlineData(0x79, NeuronType.Input, "7980")]
	[InlineData(0x79, NeuronType.Action, "7980")]
	[InlineData(0x79, NeuronType.Internal, "7900")]
	public void ToHex_ShouldHaveCorrectHexValue_ForValidParams(ushort id, NeuronType type, string expected) {
		//Arrange
		var neuron = new Neuron(id, type);

		//Act
		var b = neuron.ToHex();

		//Assert
		b.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0x7F, NeuronType.Internal, NeuronType.Input, "7F00")]
	[InlineData(0x70, NeuronType.Internal, NeuronType.Input,"7000")]
	[InlineData(0x79, NeuronType.Input, NeuronType.Input,"7980")]
	[InlineData(0x79, NeuronType.Action, NeuronType.Action,"7980")]
	public void FromHex_ShouldHaveCorrectNeuron_ForValidHex(ushort id, NeuronType type,NeuronType externalType ,string hex) {
		//Arrange

		//Act
		var neuron = Neuron.FromHex(hex, externalType);

		//Assert
		neuron.Id.Should().Be(id);
		neuron.NeuronType.Should().Be(type);
	}
	
	[Theory]
	[InlineData(0x7F, NeuronType.Internal, NeuronType.Input)]
	[InlineData(0x70, NeuronType.Internal, NeuronType.Input)]
	[InlineData(0x79, NeuronType.Input, NeuronType.Input)]
	[InlineData(0x79, NeuronType.Action, NeuronType.Action)]
	public void FromHex_ShouldHaveCorrectNeuron_WhenEncodedBefore(ushort id, NeuronType type, NeuronType externalType) {
		//Arrange
		var original = new Neuron(id, type);
		var hex = original.ToHex();

		//Act
		var neuron = Neuron.FromHex(hex, externalType);

		//Assert
		neuron.Should().Be(original);
	}
	
	
}
