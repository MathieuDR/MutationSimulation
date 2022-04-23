using System;
using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class GenomeValidationTests {

	[Fact]
	public void Ctor_ShouldCreateGenome_WhenGivenValidParams() {
		//Arrange
		var n1 = new Neuron(1, NeuronType.Input);
		var n2 = new Neuron(2, NeuronType.Internal);

		//Act
		var genome = new Genome(n1, n2, float.MaxValue);

		//Assert
		genome.Should().NotBeNull();
	}

	[Theory]
	[InlineData(NeuronType.Input, NeuronType.Input)]
	[InlineData(NeuronType.Internal, NeuronType.Input)]
	[InlineData(NeuronType.Output, NeuronType.Input)]
	[InlineData(NeuronType.Output, NeuronType.Output)]
	[InlineData(NeuronType.Output, NeuronType.Internal)]
	public void Ctor_ShouldThrowArgumentException_WhenWrongNeuronTypesConnectToEachOther(NeuronType neuronType1, NeuronType neuronType2) {
		//Arrange
		var n1 = new Neuron(1, neuronType1);
		var n2 = new Neuron(2, neuronType2);

		//Act
		Action act = () => new Genome(n1, n2, 1f);

		//Assert
		act.Should().Throw<ArgumentException>();
	}
	
	[Theory]
	[InlineData(NeuronType.Input, NeuronType.Input)]
	[InlineData(NeuronType.Internal, NeuronType.Input)]
	[InlineData(NeuronType.Output, NeuronType.Input)]
	[InlineData(NeuronType.Output, NeuronType.Output)]
	[InlineData(NeuronType.Output, NeuronType.Internal)]
	public void WithKeyword_ShouldThrowArgumentException_WhenWrongNeuronTypesConnectToEachOther(NeuronType neuronType1, NeuronType neuronType2 ) {
		//Arrange
		var n1 = new Neuron(1, NeuronType.Input);
		var n2 = new Neuron(2, NeuronType.Internal);
		var genome = new Genome(n1, n2, 1f);
		
		var n3 = new Neuron(1, neuronType1);
		var n4 = new Neuron(2, neuronType2);

		//Act
		Action act = () => genome = genome with {Source = n3, Destination = n4};

		//Assert
		act.Should().Throw<ArgumentException>();
	}
	
	[Theory]
	[InlineData(NeuronType.Input, NeuronType.Internal)]
	[InlineData(NeuronType.Internal, NeuronType.Internal)]
	[InlineData(NeuronType.Internal, NeuronType.Output)]
	public void Ctor_ShouldNotThrowArgumentException_WhenCorrectNeuronTypesConnectToEachOther(NeuronType neuronType1, NeuronType neuronType2) {
		//Arrange
		var n1 = new Neuron(1, neuronType1);
		var n2 = new Neuron(2, neuronType2);

		//Act
		Action act = () => new Genome(n1, n2, 1f);

		//Assert
		act.Should().NotThrow();
	}
	
	[Theory]
	[InlineData(NeuronType.Input, NeuronType.Internal)]
	[InlineData(NeuronType.Internal, NeuronType.Internal)]
	[InlineData(NeuronType.Internal, NeuronType.Output)]
	public void WithKeyword_ShouldNotThrowArgumentException_WhenCorrectNeuronTypesConnectToEachOther(NeuronType neuronType1, NeuronType neuronType2 ) {
		//Arrange
		var n1 = new Neuron(1, NeuronType.Input);
		var n2 = new Neuron(2, NeuronType.Internal);
		var genome = new Genome(n1, n2, 1f);
		
		var n3 = new Neuron(1, neuronType1);
		var n4 = new Neuron(2, neuronType2);

		//Act
		Action act = () => genome = genome with {Source = n3, Destination = n4};

		//Assert
		act.Should().NotThrow();
	}

	[Theory]
	[InlineData(float.MaxValue)]
	[InlineData(float.MinValue)]
	[InlineData(2134.324f)]
	[InlineData(-2134.324f)]
	[InlineData(4.00001f)]
	[InlineData(-4.00001f)]
	public void Ctor_ShouldPutWeightInRange_WhenWeightIsOutOfRange(float weight) {
		//Arrange
		var n1 = new Neuron(1, NeuronType.Input);
		var n2 = new Neuron(2, NeuronType.Internal);

		//Act
		var genome = new Genome(n1, n2, weight);

		//Assert
		genome.Weight.Should().BeInRange(-4f, 4f);
	}
	
	[Theory]
	[InlineData(float.MaxValue)]
	[InlineData(float.MinValue)]
	[InlineData(2134.324f)]
	[InlineData(-2134.324f)]
	[InlineData(4.00001f)]
	[InlineData(-4.00001f)]
	public void WithKeyword_ShouldPutWeightInRange_WhenWeightIsOutOfRange(float weight) {
		//Arrange
		var n1 = new Neuron(1, NeuronType.Input);
		var n2 = new Neuron(2, NeuronType.Internal);
		var genome = new Genome(n1, n2, 1f);

		//Act
		genome = genome with { Weight = weight };

		//Assert
		genome.Weight.Should().BeInRange(-4f, 4f);
	}

	[Fact]
	public void Ctor_ShouldHaveWeight0_WhenGivenWeight0() {
		//Arrange
		var n1 = new Neuron(1, NeuronType.Input);
		var n2 = new Neuron(2, NeuronType.Internal);

		//Act
		var genome = new Genome(n1, n2, 0f);

		//Assert
		genome.Weight.Should().Be(0f);
	}
}
