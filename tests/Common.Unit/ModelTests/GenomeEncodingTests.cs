using System;
using Common.Helpers;
using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class GenomeEncodingTests {
	
	[Fact]
	public void ToString_ShouldBe16CharsLong_WhenGivenValidGenome() {
		//Arrange
		var genome = new Genome(new Neuron(1, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f);

		//Act
		var hex = genome.ToString();
		
		//Assert
		// Neuron is short (2bytes): 4 Char
		// weight is float (4bytes): 8 Char
		hex.Should().HaveLength(16);
	}
	
	[Fact]
	public void ToString_ShouldProvideCorrectOrderHexString_WhenGivenValidInputOutputGenome() {
		//Arrange
		var n1 = new Neuron(1, NeuronType.Input);
		var n2 = new Neuron(2, NeuronType.Output);
		var genome = new Genome(n1, n2, 2f);
		var n1Hex = n1.ToString();
		var n2Hex = n2.ToString();
		var weightInHex = (genome.Weight * (float.MaxValue/4)).ToHex();
		
		var expectedHex = n1Hex + n2Hex + weightInHex;

		//Act
		var hex = genome.ToString();
		
		//Assert
		hex.Should().Be(expectedHex);
	}
	
	[Fact]
	public void ToString_ShouldProvideCorrectOrderHexString_WhenGivenValidInputInternalGenome() {
		//Arrange
		var n1 = new Neuron(1, NeuronType.Input);
		var n2 = new Neuron(2, NeuronType.Internal);
		var genome = new Genome(n1, n2, 3f);
		var n1Hex = n1.ToString();
		var n2Hex = n2.ToString();
		var weightInHex = (genome.Weight * (float.MaxValue/4)).ToHex();
		
		var expectedHex = n1Hex + n2Hex + weightInHex;

		//Act
		var hex = genome.ToString();
		
		//Assert
		hex.Should().Be(expectedHex);
	}
	
	[Fact]
	public void ToString_ShouldProvideCorrectHexString_WhenGivenValidGenome() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new Genome(n1, n2, -300f);
		var n1Hex = "3280";
		var n2Hex = "8403";
		var weightInHex = "000096C3";
		
		var expectedHex =  n1Hex + n2Hex + weightInHex;

		//Act
		var hex = genome.ToString();
		
		//Assert
		hex.Should().Be(expectedHex);
	}
	
	[Fact]
	public void ToString_ShouldProvideCorrectHexString_WhenGivenValidInputOutputGenome() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Output);
		var genome = new Genome(n1, n2, -300f);
		var n1Hex = "3280";
		var n2Hex = "8483";
		var weightInHex = "000096C3";
		
		var expectedHex =  n1Hex + n2Hex + weightInHex;

		//Act
		var hex = genome.ToString();
		
		//Assert
		hex.Should().Be(expectedHex);
	}

	
}
