using System;
using System.Linq;
using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests;

public class GenomeSequenceTests {
	[Fact]
	public void Ctor_ShouldCreateGenomeSequence_WhenProvidedGenomes() {
		//Arrange
		//Act
		var sequence = new GenomeSequence(new[] {
			new Genome(new Neuron(1, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f)
		});

		//Assert
		sequence.Should().NotBeNull();
	}
	
	[Fact]
	public void Ctor_ShouldThrowArgumentException_WhenGenomesAreEmpty() {
		//Arrange
		//Act
		var act = () =>  _ = new GenomeSequence(Array.Empty<Genome>());

		//Assert
		act.Should().Throw<ArgumentException>();
	}
	
	[Fact]
	public void WithKeyword_ShouldThrowArgumentException_WhenGenomesAreEmpty() {
		//Arrange
		var sequence = new GenomeSequence(new[] {
			new Genome(new Neuron(1, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f)
		});
		
		//Act
		var act = () => _ = sequence with { Genomes = Array.Empty<Genome>() };

		//Assert
		act.Should().Throw<ArgumentException>();
	}
	
	[Fact]
	public void GetBytes_ShouldGiveCorrectByte_WhenGivenValidSequence() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new Genome(n1, n2, -300f);
		var genome2 = new Genome(n2, n2, 100f);
		var sequence = new GenomeSequence(new[] { genome, genome2 });
		var genome1Bytes = genome.GetBytes();
		var genome2Bytes = genome2.GetBytes();
		var expectedArr = genome1Bytes.Concat(genome2Bytes).ToArray();

		//Act
		var bytes = sequence.GetBytes();

		//Assert
		bytes.Should().BeEquivalentTo(expectedArr);
	}

	[Fact]
	public void ToString_ShouldGiveCorrectHex_WhenGivenValidSequence() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new Genome(n1, n2, -300f);
		var genome2 = new Genome(n2, n2, 100f);
		var sequence = new GenomeSequence(new[] { genome, genome2 });
		var genome1Hex = genome.ToString();
		var genome2Hex = genome2.ToString();
		
		//8032 0384 C396 0000 - 0384 0384 42C8 0000
		//0000 96C3 8403 3280 - 0000 C842 8403 8403
		

		//Act
		var hex = sequence.ToString();

		//Assert
		hex.Should().Be(genome1Hex + genome2Hex);
	}
	
	[Fact]
	public void ToString_ShouldGiveCorrectHex_WhenGivenValidSequenceWithOneGenome() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new Genome(n1, n2, -300f);
		var sequence = new GenomeSequence(new[] { genome });
		var genome1Hex = genome.ToString();
		
		//8032 0384 C396 0000 - 0384 0384 42C8 0000
		//0000 96C3 8403 3280 - 0000 C842 8403 8403
		

		//Act
		var hex = sequence.ToString();

		//Assert
		hex.Should().Be(genome1Hex);
	}
}
