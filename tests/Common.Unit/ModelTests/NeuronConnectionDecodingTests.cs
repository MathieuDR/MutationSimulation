using System;
using System.Collections.Generic;
using System.Text;
using Common.Helpers;
using Common.Models;
using Common.Models.Bio;
using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

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
		result.Should().Be(neuronConnection);
	}
	
	public static IEnumerable<object[]> NeuronConnections =>
		new List<object[]>
		{
			new object[] { new Neuron(1, NeuronType.Input), new Neuron(21, NeuronType.Action), 1232f },
			new object[] { new Neuron(1000, NeuronType.Input), new Neuron(12, NeuronType.Action), 882f },
			new object[] { new Neuron(214, NeuronType.Input), new Neuron(992, NeuronType.Action), -293.51f },
			new object[] { new Neuron(112, NeuronType.Internal), new Neuron(223, NeuronType.Internal), 0.5f },
			new object[] { new Neuron(13, NeuronType.Internal), new Neuron(93, NeuronType.Action), 0.5f },
			new object[] { new Neuron(8, NeuronType.Input), new Neuron(2123, NeuronType.Internal), 0.5f },
		};
	

	[Fact]
	public void FromHex_ShouldResultInNeuronConnection_WhenGivenValidHex() {
		//Arrange
		var hex = "00B0124581800880";

		//Act
		var neuronConnection = NeuronConnection.FromHex(hex);
		
		//Assert
		neuronConnection.Should().NotBeNull();
	}
	
	[Fact]
	public void FromHex_ShouldResultInCorrectInputNeuron_WhenGivenValidHex() {
		//Arrange
		var hex = "0880158000009A44";

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
		var hex = "1900158000009A44";

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
		var hex = "0880818000009A44";

		//Act
		var neuronConnection = NeuronConnection.FromHex(hex);
		var sourceNeuron = neuronConnection.Target;
		
		//Assert
		sourceNeuron.NeuronType.Should().Be(NeuronType.Action);
		sourceNeuron.Id.Should().Be(129);
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
