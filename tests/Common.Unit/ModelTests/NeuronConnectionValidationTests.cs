using System;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class NeuronConnectionValidationTests {

	[Fact]
	public void Ctor_ShouldCreateNeuronConnection_WhenGivenValidParams() {
		//Arrange
		var n1 = new Neuron(1, 1f, NeuronType.Input);
		var n2 = new Neuron(2, 2f, NeuronType.Internal);

		//Act
		var neuronConnection = new NeuronConnection(n1, n2, float.MaxValue);

		//Assert
		neuronConnection.Should().NotBeNull();
	}

	[Theory]
	[InlineData(NeuronType.Input, NeuronType.Input)]
	[InlineData(NeuronType.Internal, NeuronType.Input)]
	[InlineData(NeuronType.Action, NeuronType.Input)]
	[InlineData(NeuronType.Action, NeuronType.Action)]
	[InlineData(NeuronType.Action, NeuronType.Internal)]
	public void Ctor_ShouldThrowArgumentException_WhenWrongNeuronTypesConnectToEachOther(NeuronType neuronType1, NeuronType neuronType2) {
		//Arrange
		var n1 = new Neuron(1, 1f, neuronType1);
		var n2 = new Neuron(2, 9f, neuronType2);

		//Act
		Action act = () => new NeuronConnection(n1, n2, 1f);

		//Assert
		act.Should().Throw<ArgumentException>();
	}
	
	[Theory]
	[InlineData(NeuronType.Input, NeuronType.Input)]
	[InlineData(NeuronType.Internal, NeuronType.Input)]
	[InlineData(NeuronType.Action, NeuronType.Input)]
	[InlineData(NeuronType.Action, NeuronType.Action)]
	[InlineData(NeuronType.Action, NeuronType.Internal)]
	public void WithKeyword_ShouldThrowArgumentException_WhenWrongNeuronTypesConnectToEachOther(NeuronType neuronType1, NeuronType neuronType2 ) {
		//Arrange
		var n1 = new Neuron(1, 0f, NeuronType.Input);
		var n2 = new Neuron(2, 0f, NeuronType.Internal);
		var neuronConnection = new NeuronConnection(n1, n2, 1f);
		
		var n3 = new Neuron(1, 0f, neuronType1);
		var n4 = new Neuron(2, 0f, neuronType2);

		//Act
		Action act = () => neuronConnection = neuronConnection with {Source = n3, Target = n4};

		//Assert
		act.Should().Throw<ArgumentException>();
	}
	
	[Theory]
	[InlineData(NeuronType.Input, NeuronType.Internal)]
	[InlineData(NeuronType.Internal, NeuronType.Internal)]
	[InlineData(NeuronType.Internal, NeuronType.Action)]
	public void Ctor_ShouldNotThrowArgumentException_WhenCorrectNeuronTypesConnectToEachOther(NeuronType neuronType1, NeuronType neuronType2) {
		//Arrange
		var n1 = new Neuron(1, 0f, neuronType1);
		var n2 = new Neuron(2, 0f, neuronType2);

		//Act
		Action act = () => new NeuronConnection(n1, n2, 1f);

		//Assert
		act.Should().NotThrow();
	}
	
	[Theory]
	[InlineData(NeuronType.Input, NeuronType.Internal)]
	[InlineData(NeuronType.Internal, NeuronType.Internal)]
	[InlineData(NeuronType.Internal, NeuronType.Action)]
	public void WithKeyword_ShouldNotThrowArgumentException_WhenCorrectNeuronTypesConnectToEachOther(NeuronType neuronType1, NeuronType neuronType2 ) {
		//Arrange
		var n1 = new Neuron(1, 0f, NeuronType.Input);
		var n2 = new Neuron(2, 0f, NeuronType.Internal);
		var neuronConnection = new NeuronConnection(n1, n2, 1f);
		
		var n3 = new Neuron(1, 0f, neuronType1);
		var n4 = new Neuron(2, 0f, neuronType2);

		//Act
		Action act = () => neuronConnection = neuronConnection with {Source = n3, Target = n4};

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
		var n1 = new Neuron(1, 0f, NeuronType.Input);
		var n2 = new Neuron(2, 0f, NeuronType.Internal);

		//Act
		var neuronConnection = new NeuronConnection(n1, n2, weight);

		//Assert
		neuronConnection.Weight.Should().BeInRange(-4f, 4f);
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
		var n1 = new Neuron(1, 0f, NeuronType.Input);
		var n2 = new Neuron(2, 0f, NeuronType.Internal);
		var neuronConnection = new NeuronConnection(n1, n2, 1f);

		//Act
		neuronConnection = neuronConnection with { Weight = weight };

		//Assert
		neuronConnection.Weight.Should().BeInRange(-4f, 4f);
	}

	[Fact]
	public void Ctor_ShouldHaveWeight0_WhenGivenWeight0() {
		//Arrange
		var n1 = new Neuron(1, 0f, NeuronType.Input);
		var n2 = new Neuron(2, 0f, NeuronType.Internal);

		//Act
		var neuronConnection = new NeuronConnection(n1, n2, 0f);

		//Assert
		neuronConnection.Weight.Should().Be(0f);
	}
}
