using System.Collections.Generic;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class NeuronConnectionDecodingTests {
	[Theory]
	[MemberData(nameof(NeuronConnections))]
	public void FromHex_ShouldResultInSameNeuronConnection_WhenGivenNeuronConnection(Neuron sourceNeuron, Neuron destinationNeuron, float weight) {
		//Arrange
		var neuronConnection = new NeuronConnection(sourceNeuron, destinationNeuron, weight);
		var hex = neuronConnection.ToHex();

		//Act
		var result = NeuronConnection.FromHex(hex);

		//Assert
		result.Should().BeEquivalentTo(neuronConnection);
	}
	
	public static IEnumerable<object[]> NeuronConnections =>
		new List<object[]>
		{
			new object[] { new Neuron(1, NeuronType.Input), new Neuron(1, NeuronType.Action), 1232f },
			new object[] { new Neuron(5, NeuronType.Input), new Neuron(3, NeuronType.Action), 882f },
			new object[] { new Neuron(3, NeuronType.Input), new Neuron(4, NeuronType.Action), -293.51f },
			new object[] { new Neuron(112, NeuronType.Internal), new Neuron(223, NeuronType.Internal), 0.5f },
			new object[] { new Neuron(13, NeuronType.Internal), new Neuron(2, NeuronType.Action), 0.5f },
			new object[] { new Neuron(8, NeuronType.Input), new Neuron(2123, NeuronType.Internal), 0.5f },
		};
	

	[Fact]
	public void FromHex_ShouldResultInNeuronConnection_WhenGivenValidHex() {
		//Arrange
		var hex = "19000980FFFF3F7F";

		//Act
		var neuronConnection = NeuronConnection.FromHex(hex);
		
		//Assert
		neuronConnection.Should().NotBeNull();
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectInputNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "08800580FFFF3F7F";

		//Act
		var neuronConnection = NeuronConnection.FromHex(hex);
		var sourceNeuron = neuronConnection.Source;
		
		//Assert
		sourceNeuron.NeuronType.Should().Be(NeuronType.Input);
		sourceNeuron.Id.Should().Be(8);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectSourceInternalNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "19000580FFFF3F7F";

		//Act
		var neuronConnection = NeuronConnection.FromHex(hex);
		var sourceNeuron = neuronConnection.Source;
		
		//Assert
		sourceNeuron.NeuronType.Should().Be(NeuronType.Internal);
		sourceNeuron.Id.Should().Be(25);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectOutputNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "19000980FFFF3F7F";

		//Act
		var neuronConnection = NeuronConnection.FromHex(hex);
		var resultNeuron = neuronConnection.Target;
		
		//Assert
		resultNeuron.NeuronType.Should().Be(NeuronType.Action);
		resultNeuron.Id.Should().Be(9);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectDestinationInternalNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "0880421C00009A44";

		//Act
		var neuronConnection = NeuronConnection.FromHex(hex);
		var sourceNeuron = neuronConnection.Target;
		
		//Assert
		sourceNeuron.NeuronType.Should().Be(NeuronType.Internal);
		sourceNeuron.Id.Should().Be(7234);
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectWeight_WhenGivenValidHexAndInRange() {
		//Arrange
		var hex = "0880421CFFFF5F7F";

		//Act
		var neuronConnection = NeuronConnection.FromHex(hex);
		
		//Assert
		neuronConnection.Weight.Should().BeInRange(3.49f, 3.51f);
	}
	
}
