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
		var firstPart = n1.ToString();
		var secondPart = n2.ToString();
		var weightInHex = (genome.Weight * (float.MaxValue/4)).ToHex();
		
		var expectedHex = firstPart + secondPart + weightInHex;

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
		var firstPart = n1.ToString();
		var secondPart = n2.ToString();
		var weightInHex = (genome.Weight * (float.MaxValue/4)).ToHex();
		
		var expectedHex = firstPart + secondPart + weightInHex;

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
		var firstPart = "8032";
		var secondPart = "0384";
		var weightInHex = "C3960000";
		
		var expectedHex = firstPart + secondPart + weightInHex;

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
		var firstPart = "8032";
		var secondPart = "8384";
		var weightInHex = "C3960000";
		
		var expectedHex = firstPart + secondPart + weightInHex;

		//Act
		var hex = genome.ToString();
		
		//Assert
		hex.Should().Be(expectedHex);
	}

	
}
