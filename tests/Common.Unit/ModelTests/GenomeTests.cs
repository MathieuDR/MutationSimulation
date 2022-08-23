using System.Linq;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using FluentAssertions;
using LZStringCSharp;
using Xunit;

namespace Common.Unit.ModelTests;

public class GenomeTests {
	[Fact]
	public void Ctor_ShouldCreateGenome_WhenProvidedNeuronConnection() {
		//Arrange
		//Act
		var sequence = new OldGenome(new[] {
			new NeuronConnection(new InputNeuron(1, 9f), new Neuron(2, 9f, NeuronType.Action), 2f)
		});

		//Assert
		sequence.Should().NotBeNull();
	}
	
	[Fact]
	public void FromHex_ShouldReturnSameGenome_WhenProvided() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new InputNeuron(1, 9f), new Neuron(1, 9f, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(1, 9f, NeuronType.Internal), new Neuron(2, 9f, NeuronType.Internal),  NeuronConnection.WeightToFloat(-1f)),
			new NeuronConnection(new Neuron(1, 2f, NeuronType.Internal), new Neuron(3, 9f, NeuronType.Internal),  NeuronConnection.WeightToFloat(1.02f)),
			new NeuronConnection(new Neuron(2, 9f, NeuronType.Internal), new Neuron(1, 9f, NeuronType.Internal), NeuronConnection.WeightToFloat(3.02f)),
			new NeuronConnection(new Neuron(2, 1f, NeuronType.Internal), new Neuron(3, 0, NeuronType.Internal), NeuronConnection.WeightToFloat(-1.02f)),
			new NeuronConnection(new Neuron(3, 9f, NeuronType.Internal), new Neuron(4, 9f, NeuronType.Internal), NeuronConnection.WeightToFloat(3.29f)),
			new NeuronConnection(new Neuron(3, 9f, NeuronType.Internal), new Neuron(1, 9f, NeuronType.Internal), NeuronConnection.WeightToFloat(1.92f)),
			new NeuronConnection(new Neuron(4, 1f, NeuronType.Internal), new Neuron(1, 9f, NeuronType.Internal), NeuronConnection.WeightToFloat(1.02f)),
			new NeuronConnection(new Neuron(4, 9f, NeuronType.Internal), new Neuron(5, -23f, NeuronType.Internal), NeuronConnection.WeightToFloat(1.02f)),
			new NeuronConnection(new Neuron(5, 9f, NeuronType.Internal), new ActionNeuron(1, 9f), NeuronConnection.WeightToFloat(-2.73f)),
			new NeuronConnection(new Neuron(5, 9f, NeuronType.Internal), new Neuron(2, 119f, NeuronType.Action), NeuronConnection.WeightToFloat(1.02f)),
			new NeuronConnection(new Neuron(5, 9f, NeuronType.Internal), new Neuron(3, 8f, NeuronType.Action), NeuronConnection.WeightToFloat(-1.02f)),
			new NeuronConnection(new Neuron(5, -1f, NeuronType.Internal), new Neuron(3, 9f, NeuronType.Internal), NeuronConnection.WeightToFloat(1.02f)),
		};

		var genome = new OldGenome(connections);

		//Act
		var hex = genome.ToHex();
		var genome2 = OldGenome.FromHex(hex);

		//Assert
		genome.Should().BeEquivalentTo(genome2, options => options.Excluding(x => x.HexSequence));
	}

	[Fact]
	public void CreateGenome() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new InputNeuron(1,0), new Neuron(1, 0, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(1,0, NeuronType.Internal), new Neuron(2,0, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(2,0, NeuronType.Internal), new Neuron(3,0, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(2,2f, NeuronType.Internal), new Neuron(1,0, NeuronType.Internal), NeuronConnection.WeightToFloat(1f)),
			new NeuronConnection(new Neuron(2, 0,NeuronType.Internal), new ActionNeuron(1,0), NeuronConnection.WeightToFloat(1f)),
		};

		var genome = new OldGenome(connections);

		//Act
		var hex = genome.ToHex();
		
	}

	[Fact]
	public void GetBytes_ShouldGiveCorrectByte_WhenGivenValidSequence() {
		//Arrange
		var n1 = new Neuron(50,2f, NeuronType.Input);
		var n2 = new Neuron(900,2f, NeuronType.Internal);
		var genome = new NeuronConnection(n1, n2, -300f);
		var genome2 = new NeuronConnection(n2, n2, 100f);
		var sequence = new OldGenome(new[] { genome, genome2 });
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
		var n1 = new Neuron(50,2f, NeuronType.Input);
		var n2 = new Neuron(900,2f, NeuronType.Internal);
		var genome = new NeuronConnection(n1, n2, -300f);
		var genome2 = new NeuronConnection(n2, n2, 100f);
		var sequence = new OldGenome(new[] { genome, genome2 });
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
		var n1 = new Neuron(50,2f, NeuronType.Input);
		var n2 = new Neuron(900,1f, NeuronType.Internal);
		var genome = new NeuronConnection(n1, n2, -300f);
		var genome2 = new NeuronConnection(n2, n2, 100f);
		var genome3 = new NeuronConnection(n1, n2, 800f);
		var sequence = new OldGenome(new[] { genome, genome2, genome3 });
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
		var n1 = new Neuron(50,2f, NeuronType.Input);
		var n2 = new Neuron(900,2f, NeuronType.Internal);
		var connection = new NeuronConnection(n1, n2, -300f);
		var genome = new OldGenome(new[] { connection });
		var connectionHex = connection.ToHex();

		//Act
		var hex = genome.ToHex();

		//Assert
		hex.Should().Be(LZString.CompressToBase64(connectionHex));
	}
}
