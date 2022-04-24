using System;
using System.Collections.Generic;
using System.Text;
using Common.Helpers;
using Common.Models;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Common.Unit.ModelTests; 

public class GenomeDecodingTests {
	[Theory]
	[MemberData(nameof(Genomes))]
	public void FromHex_ShouldResultInSameGenome_WhenGivenGenome(Neuron sourceNeuron, Neuron destinationNeuron, float weight) {
		//Arrange
		var genome = new Genome(sourceNeuron, destinationNeuron, weight);
		var hex = genome.ToString();

		//Act
		var result = Genome.FromHex(hex);

		//Assert
		result.Should().Be(genome);
	}
	
	public static IEnumerable<object[]> Genomes =>
		new List<object[]>
		{
			new object[] { new Neuron(1, NeuronType.Input), new Neuron(21, NeuronType.Output), 1232f },
			new object[] { new Neuron(1000, NeuronType.Input), new Neuron(12, NeuronType.Output), 882f },
			new object[] { new Neuron(214, NeuronType.Input), new Neuron(992, NeuronType.Output), -293.51f },
			new object[] { new Neuron(112, NeuronType.Internal), new Neuron(223, NeuronType.Internal), 0.5f },
			new object[] { new Neuron(13, NeuronType.Internal), new Neuron(93, NeuronType.Output), 0.5f },
			new object[] { new Neuron(8, NeuronType.Input), new Neuron(2123, NeuronType.Internal), 0.5f },
		};
	

	[Fact]
	public void FromHex_ShouldResultInGenome_WhenGivenValidHex() {
		//Arrange
		var hex = "00B0124581800880";

		//Act
		var genome = Genome.FromHex(hex);
		
		//Assert
		genome.Should().NotBeNull();
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectInputNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "0880158000009A44";

		//Act
		var genome = Genome.FromHex(hex);
		var sourceNeuron = genome.Source;
		
		//Assert
		sourceNeuron.NeuronType.Should().Be(NeuronType.Input);
		sourceNeuron.Id.Should().Be(8);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectSourceInternalNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "1900158000009A44";

		//Act
		var genome = Genome.FromHex(hex);
		var sourceNeuron = genome.Source;
		
		//Assert
		sourceNeuron.NeuronType.Should().Be(NeuronType.Internal);
		sourceNeuron.Id.Should().Be(25);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectOutputNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "0880818000009A44";

		//Act
		var genome = Genome.FromHex(hex);
		var sourceNeuron = genome.Destination;
		
		//Assert
		sourceNeuron.NeuronType.Should().Be(NeuronType.Output);
		sourceNeuron.Id.Should().Be(129);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectDestinationInternalNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "0880421C00009A44";

		//Act
		var genome = Genome.FromHex(hex);
		var sourceNeuron = genome.Destination;
		
		//Assert
		sourceNeuron.NeuronType.Should().Be(NeuronType.Internal);
		sourceNeuron.Id.Should().Be(7234);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectWeight_WhenGivenValidHexAndInRange() {
		//Arrange
		var hex = "0880421CFFFF5F7F";

		//Act
		var genome = Genome.FromHex(hex);
		
		//Assert
		genome.Weight.Should().BeInRange(3.49f, 3.51f);
	}
	
}
