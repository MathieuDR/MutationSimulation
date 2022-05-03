using System;
using System.Collections.Generic;
using System.Linq;
using Common.Models;
using FluentAssertions;
using LZStringCSharp;
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
	public void FromHex_ShouldReturnSameGenome_WhenProvided() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new Neuron(1, NeuronType.Input), new Neuron(1, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(1, NeuronType.Internal), new Neuron(2, NeuronType.Internal),  NeuronConnection.WeightToFloat(-1f)),
			new NeuronConnection(new Neuron(1, NeuronType.Internal), new Neuron(3, NeuronType.Internal),  NeuronConnection.WeightToFloat(1.02f)),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(1, NeuronType.Internal), NeuronConnection.WeightToFloat(3.02f)),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), NeuronConnection.WeightToFloat(-1.02f)),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), NeuronConnection.WeightToFloat(3.29f)),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(1, NeuronType.Internal), NeuronConnection.WeightToFloat(1.92f)),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(1, NeuronType.Internal), NeuronConnection.WeightToFloat(1.02f)),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(5, NeuronType.Internal), NeuronConnection.WeightToFloat(1.02f)),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(1, NeuronType.Output), NeuronConnection.WeightToFloat(-2.73f)),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(2, NeuronType.Output), NeuronConnection.WeightToFloat(1.02f)),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(3, NeuronType.Output), NeuronConnection.WeightToFloat(-1.02f)),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(3, NeuronType.Internal), NeuronConnection.WeightToFloat(1.02f)),
		};

		var genome = new Genome(connections);

		//Act
		var hex = genome.ToHex();
		var genome2 = Genome.FromHex(hex);

		//Assert
		genome.Should().BeEquivalentTo(genome2, options => options.Excluding(x => x.HexSequence));
	}

	[Fact]
	public void CreateGenome() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new Neuron(1, NeuronType.Input), new Neuron(1, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(1, NeuronType.Internal), new Neuron(2, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(1, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(1, NeuronType.Output), NeuronConnection.WeightToFloat(1f)),
		};

		var genome = new Genome(connections);

		//Act
		var hex = genome.ToHex();
		
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
		hex.Should().Be(LZString.CompressToBase64(genome1Hex + genome2Hex));
	}
	
	[Fact]
	public void ToHex_ShouldGiveCorrectHex_WhenGivenValidSequenceWithThreeConnections() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new NeuronConnection(n1, n2, -300f);
		var genome2 = new NeuronConnection(n2, n2, 100f);
		var genome3 = new NeuronConnection(n1, n2, 800f);
		var sequence = new Genome(new[] { genome, genome2, genome3 });
		var genome1Hex = genome.ToHex();
		var genome2Hex = genome2.ToHex();
		var genome3Hex = genome3.ToHex();
		

		//Act
		var hex = sequence.ToHex();

		//Assert
		hex.Should().Be(LZString.CompressToBase64(genome1Hex + genome2Hex + genome3Hex));
	}
	
	[Fact]
	public void ToHex_ShouldGiveCorrectHex_WhenGivenValidSequenceWithOneGenome() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var connection = new NeuronConnection(n1, n2, -300f);
		var genome = new Genome(new[] { connection });
		var connectionHex = connection.ToHex();

		//Act
		var hex = genome.ToHex();

		//Assert
		hex.Should().Be(LZString.CompressToBase64(connectionHex));
	}
}
