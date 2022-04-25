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
	public void ToString_ShouldGiveCorrectHex_WhenGivenValidSequence() {
		//Arrange
		var n1 = new Neuron(50, NeuronType.Input);
		var n2 = new Neuron(900, NeuronType.Internal);
		var genome = new NeuronConnection(n1, n2, -300f);
		var genome2 = new NeuronConnection(n2, n2, 100f);
		var sequence = new Genome(new[] { genome, genome2 });
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
		var genome = new NeuronConnection(n1, n2, -300f);
		var sequence = new Genome(new[] { genome });
		var genome1Hex = genome.ToString();
		

		//Act
		var hex = sequence.ToString();

		//Assert
		hex.Should().Be(genome1Hex);
	}
	
	[Theory]
	[MemberData(nameof(Sequences))]
	public void FromHex_ShouldResultInSameGenome_WhenGivenGenome(Genome sequence) {
		//Arrange
		var hex = sequence.ToString();

		//Act
		var result = Genome.FromHex(hex);
		
		//Assert
		result.Should().BeEquivalentTo(sequence, options => options.Excluding(x => x.HexSequence));
	}

	[Fact]
	public void Ctor_ShouldPruneNeuronConnections_WhenThereAreNoValidConnections() {
		//Arrange
		//Act
		var genome = new Genome(new[] { new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f) });

		//Assert
		genome.UsedConnections.Should().BeEmpty();
	}
	
	[Fact]
	public void Ctor_ShouldHaveOneUsedConnection_WhenThereIsInternalToOutput() {
		//Arrange
		//Act
		var genome = new Genome(new[] { new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f) });

		//Assert
		genome.UsedConnections.Should().HaveCount(1);
	}
	
	[Fact]
	public void Ctor_ShouldHaveOneUsedConnection_WhenThereIsInputToOutput() {
		//Arrange
		//Act
		var genome = new Genome(new[] { new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f) });

		//Assert
		genome.UsedConnections.Should().HaveCount(1);
	}
	
	[Fact]
	public void Ctor_ShouldHaveTwoUsedConnection_WhenThereAreTwoConnections() {
		//Arrange
		//Act
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f)
		});

		//Assert
		genome.UsedConnections.Should().HaveCount(2);
	}
	
	[Fact]
	public void Ctor_ShouldHaveThreeUsedConnection_WhenInternalsAreConnectedToEachother() {
		//Arrange
		//Act
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f)
		});

		//Assert
		genome.UsedConnections.Should().HaveCount(3);
	}

	
	[Fact]
	public void Ctor_ShouldPrune_WhenInternalIsSplitting() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(5, NeuronType.Internal), 2f)
		};
		//Act
		var genome = new Genome(connections);

		//Assert
		genome.UsedConnections.Should().HaveCount(3);
	}

	[Fact]
	public void Ctor_ShouldHaveTwoUsedConnection_WhenThereAreTwoConnectionsAndNonUsed() {
		//Arrange
		//Act
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(9, NeuronType.Internal), 2f)
		});

		//Assert
		genome.UsedConnections.Should().HaveCount(2);
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
	
	public static NeuronConnection[] NeuronConnections =>
		new NeuronConnection[]
		{
			new(Neurons[0], Neurons[2], NeuronConnection.WeightToFloat(3f)),
			new(Neurons[2], Neurons[1], NeuronConnection.WeightToFloat(3f)),
			new(Neurons[2], Neurons[4], NeuronConnection.WeightToFloat(-2.2f)),
			new(Neurons[3], Neurons[2], NeuronConnection.WeightToFloat(-0.4f)),
		};
	
	public static IEnumerable<object[]> Sequences =>
		new List<object[]>
		{
			new object[] { new Genome(new []{NeuronConnections[0], NeuronConnections[1],NeuronConnections[2] }) },
			new object[] { new Genome(new []{NeuronConnections[3], NeuronConnections[1],NeuronConnections[2] }) },
			new object[] { new Genome(new []{NeuronConnections[1],NeuronConnections[2] }) },
		};
}
