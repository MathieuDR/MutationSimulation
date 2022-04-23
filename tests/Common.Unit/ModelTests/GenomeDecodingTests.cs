using System;
using Common.Helpers;
using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class GenomeDecodingTests {

	[Fact]
	public void FromHex_ShouldResultInGenome_WhenGivenValidHex() {
		//Arrange
		var hex = "800880814512B000";

		//Act
		var genome = Genome.FromHex(hex);
		
		//Assert
		genome.Should().NotBeNull();
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectInputNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "800880814512B000";

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
		var hex = "001980814512B000";

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
		var hex = "800880814512B000";

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
		var hex = "00191C424512B000";

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
		var hex = "00191C4240600000";

		//Act
		var genome = Genome.FromHex(hex);
		
		//Assert
		genome.Weight.Should().Be(3.5f);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectWeight_WhenGivenValidHexAndInNotRange() {
		//Arrange
		var hex = "00191C4240DCCCCD";

		//Act
		var genome = Genome.FromHex(hex);

		//Assert
		genome.Weight.Should().Be(6.900000095367431640625f);
	}
}
