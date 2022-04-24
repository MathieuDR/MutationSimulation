using System;
using System.Collections.Generic;
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
		

		//Act
		var hex = sequence.ToString();

		//Assert
		hex.Should().Be(genome1Hex);
	}
	
	[Theory]
	[MemberData(nameof(Sequences))]
	public void FromHex_ShouldResultInSameGenomeSequence_WhenGivenGenomeSequence(GenomeSequence sequence) {
		//Arrange
		var hex = sequence.ToString();

		//Act
		var result = GenomeSequence.FromHex(hex);
		
		//Assert
		result.Should().BeEquivalentTo(sequence, options => options.Excluding(x => x.HexSequence));
	}
	
	public static Neuron[] Neurons =>
		new Neuron[]
		{
			new (1, NeuronType.Input), 
			new (1, NeuronType.Output),
			new (1, NeuronType.Internal),
			new (2, NeuronType.Input),
			new (2, NeuronType.Output)
		};
	
	public static Genome[] Genomes =>
		new Genome[]
		{
			new(Neurons[0], Neurons[2], Genome.WeightToFloat(3f)),
			new(Neurons[2], Neurons[1], Genome.WeightToFloat(3f)),
			new(Neurons[2], Neurons[4], Genome.WeightToFloat(-2.2f)),
			new(Neurons[3], Neurons[2], Genome.WeightToFloat(-0.4f)),
		};
	
	public static IEnumerable<object[]> Sequences =>
		new List<object[]>
		{
			new object[] { new GenomeSequence(new []{Genomes[0], Genomes[1],Genomes[2] }) },
			new object[] { new GenomeSequence(new []{Genomes[3], Genomes[1],Genomes[2] }) },
			new object[] { new GenomeSequence(new []{Genomes[1],Genomes[2] }) },
		};

}
