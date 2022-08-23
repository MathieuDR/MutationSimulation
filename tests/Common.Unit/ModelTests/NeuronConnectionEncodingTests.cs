using Common.Helpers;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class NeuronConnectionEncodingTests {
	
	[Fact]
	public void ToHex_ShouldBe16CharsLong_WhenGivenValidNeuronConnection() {
		//Arrange
		var neuronConnection = new NeuronConnection(new Neuron(1, 3f, NeuronType.Input), new Neuron(2,0 , NeuronType.Action), 2f);

		//Act
		var hex = neuronConnection.ToHex();
		
		//Assert
		// Neuron is short (2bytes): 4 Char = 8
		// weight is float (4bytes): 8 Char = 8
		// bias is float (4 bytes): 8 char = 16
		hex.Should().HaveLength(32);
	}
	
	[Fact]
	public void ToHex_ShouldProvideCorrectOrderHexString_WhenGivenValidInputOutputNeuronConnection() {
		//Arrange
		var n1 = new Neuron(1, 0, NeuronType.Input);
		var n2 = new Neuron(2, 1f, NeuronType.Action);
		var neuronConnection = new NeuronConnection(n1, n2, 2f);
		var n1Hex = n1.ToHex();
		var n2Hex = n2.ToHex();
		var weightInHex = (neuronConnection.Weight * (float.MaxValue/4)).ToHex();
		
		var expectedHex = n1Hex + n2Hex + weightInHex;

		//Act
		var hex = neuronConnection.ToHex();
		
		//Assert
		hex.Should().Be(expectedHex);
	}
	
	[Fact]
	public void ToHex_ShouldProvideCorrectOrderHexString_WhenGivenValidInputInternalNeuronConnection() {
		//Arrange
		var n1 = new Neuron(1, 9f, NeuronType.Input);
		var n2 = new Neuron(2, 2f, NeuronType.Internal);
		var neuronConnection = new NeuronConnection(n1, n2, 3f);
		var n1Hex = n1.ToHex();
		var n2Hex = n2.ToHex();
		var weightInHex = (neuronConnection.Weight * (float.MaxValue/4)).ToHex();
		
		var expectedHex = n1Hex + n2Hex + weightInHex;

		//Act
		var hex = neuronConnection.ToHex();
		
		//Assert
		hex.Should().Be(expectedHex);
	}
	
	[Fact]
	public void ToHex_ShouldProvideCorrectHexString_WhenGivenValidNeuronConnection() {
		//Arrange
		var n1 = new Neuron(50, 0f, NeuronType.Input);
		var n2 = new Neuron(900, 0f, NeuronType.Internal);
		var neuronConnection = new NeuronConnection(n1, n2, -300f);
		var n1Hex = "3280";
		var n2Hex = "8403";
		var weightInHex = "000096C3";
		
		var expectedHex =  n1Hex + n2Hex + weightInHex;

		//Act
		var hex = neuronConnection.ToHex();
		
		//Assert
		hex.Should().Be(expectedHex);
	}
	
	[Fact]
	public void ToHex_ShouldProvideCorrectHexString_WhenGivenValidInputOutputNeuronConnection() {
		//Arrange
		var n1 = new Neuron(50, 1f, NeuronType.Input);
		var n2 = new Neuron(900, 2f, NeuronType.Action);
		var neuronConnection = new NeuronConnection(n1, n2, -300f);
		var n1Hex = "3280";
		var n2Hex = "8483";
		var weightInHex = "000096C3";
		
		var expectedHex =  n1Hex + n2Hex + weightInHex;

		//Act
		var hex = neuronConnection.ToHex();
		
		//Assert
		hex.Should().Be(expectedHex);
	}

	
}
