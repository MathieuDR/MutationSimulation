using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests;

public class GenomeTests {
	[Fact]
	public void Ctor_ShouldCreateGenome_WhenProvidedNeuronConnection() {
		//Arrange
		//Act
		var sequence = new Genome(new[] {
			new NeuronConnection(new Neuron(1, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f)
		});

		//Assert
		sequence.Should().NotBeNull();
	}
	
	[Fact]
	public void Ctor_ShouldThrowArgumentException_WhenNeuronConnectionAreEmpty() {
		//Arrange
		//Act
		var act = () =>  _ = new Genome(Array.Empty<NeuronConnection>());

		//Assert
		act.Should().Throw<ArgumentException>();
	}
	
	[Fact]
	public void WithKeyword_ShouldThrowArgumentException_WhenNeuronConnectionAreEmpty() {
		//Arrange
		var sequence = new Genome(new[] {
			new NeuronConnection(new Neuron(1, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f)
		});
		
		//Act
		var act = () => _ = sequence with { NeuronConnections = Array.Empty<NeuronConnection>() };

		//Assert
		act.Should().Throw<ArgumentException>();
	}
	
	[Fact]
	public void GetBytes_ShouldGiveCorrectByte_WhenGivenValidSequence() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new NeuronConnection(n1, n2, -300f);
		var genome2 = new NeuronConnection(n2, n2, 100f);
		var sequence = new Genome(new[] { genome, genome2 });
		var genome1Bytes = genome.GetBytes();
		var genome2Bytes = genome2.GetBytes();
		var expectedArr = genome1Bytes.Concat(genome2Bytes).ToArray();

		//Act
		var bytes = sequence.GetBytes();

		//Assert
		bytes.Should().BeEquivalentTo(expectedArr);
	}

	[Fact]
	public void ToHex_ShouldGiveCorrectHex_WhenGivenValidSequence() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new NeuronConnection(n1, n2, -300f);
		var genome2 = new NeuronConnection(n2, n2, 100f);
		var sequence = new Genome(new[] { genome, genome2 });
		var genome1Hex = genome.ToHex();
		var genome2Hex = genome2.ToHex();
		

		//Act
		var hex = sequence.ToHex();

		//Assert
		hex.Should().Be(genome1Hex + genome2Hex);
	}
	
	[Fact]
	public void ToHex_ShouldGiveCorrectHex_WhenGivenValidSequenceWithOneGenome() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new NeuronConnection(n1, n2, -300f);
		var sequence = new Genome(new[] { genome });
		var genome1Hex = genome.ToHex();
		

		//Act
		var hex = sequence.ToHex();

		//Assert
		hex.Should().Be(genome1Hex);
	}
}
