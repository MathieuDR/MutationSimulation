using Common.Models.Genetic;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests;

public class BrainTests {
    [Fact]
    public void Brain_ShouldHave4SortedNeurons_WhenThereAre2SeparatePaths() {
        //Arrange
        var genome = new OldGenome(new[] {
            new NeuronConnection(new InputNeuron(2, 9f), new ActionNeuron(2, 9f), 2f),
            new NeuronConnection(new InputNeuron(3, 9f), new ActionNeuron(3, 9f), 2f)
        });

        //Act
        var brain = new Brain(genome);

        //Assert
        brain.SortedNeurons.Should().HaveCount(4);
    }

    [Fact]
    public void Brain_ShouldHave3Neurons_WhenThereIsSplittingPath() {
        //Arrange
        var genome = new OldGenome(new[] {
            new NeuronConnection(new InputNeuron(2, 9f), new ActionNeuron(2, 9f), 2f),
            new NeuronConnection(new InputNeuron(2, 9f), new ActionNeuron(3, 9f), 2f)
        });

        //Act
        var brain = new Brain(genome);

        //Assert
        brain.SortedNeurons.Should().HaveCount(3);
    }

    [Fact]
    public void Brain_ShouldKeepOutputs_WhenPruning() {
        //Arrange
        var connections = new[] {
            new NeuronConnection(new InputNeuron(2, 9f), new Neuron(2, 9f, NeuronType.Internal), 2f),
            new NeuronConnection(new Neuron(2, 9f, NeuronType.Internal), new Neuron(3, 9f, NeuronType.Internal), 2f),
            new NeuronConnection(new Neuron(3, 9f, NeuronType.Internal), new ActionNeuron(3, 9f), 2f),
            new NeuronConnection(new Neuron(4, 9f, NeuronType.Internal), new Neuron(2, 9f, NeuronType.Internal), 2f),
            new NeuronConnection(new Neuron(4, 9f, NeuronType.Internal), new ActionNeuron(2, 9f), 2f),
            new NeuronConnection(new Neuron(3, 9f, NeuronType.Internal), new Neuron(4, 9f, NeuronType.Internal), 2f),
        };

        var genome = new OldGenome(connections);

        //Act
        var brain = new Brain(genome);

        //Assert
        brain.SortedNeurons.Should().Contain(connections[2].Target);
        brain.SortedNeurons.Should().Contain(connections[4].Target);
    }

    [Fact]
    public void Brain_ShouldKeepInputs_WhenPruning() {
        //Arrange
        var connections = new[] {
            new NeuronConnection(new InputNeuron(2, 9f), new Neuron(2, 9f, NeuronType.Internal), 2f),
            new NeuronConnection(new Neuron(2, 9f, NeuronType.Internal), new Neuron(3, 9f, NeuronType.Internal), 2f),
            new NeuronConnection(new Neuron(3, 9f, NeuronType.Internal), new ActionNeuron(3, 9f), 2f),
            new NeuronConnection(new Neuron(4, 9f, NeuronType.Internal), new Neuron(2, 9f, NeuronType.Internal), 2f),
            new NeuronConnection(new Neuron(4, 9f, NeuronType.Internal), new ActionNeuron(2, 9f), 2f),
            new NeuronConnection(new Neuron(3, 9f, NeuronType.Internal), new Neuron(4, 9f, NeuronType.Internal), 2f),
        };

        var genome = new OldGenome(connections);

        //Act
        var brain = new Brain(genome);

        //Assert
        brain.SortedNeurons.Should().Contain(connections[0].Source);
    }

    [Fact]
    public void Brain_ShouldAddMemory_WhenSelfReferencing() {
        //Arrange
        var genome = new OldGenome(new[] {
            new NeuronConnection(new InputNeuron(2, 9f), new Neuron(2, 9f, NeuronType.Internal), 2f),
            new NeuronConnection(new Neuron(2, 9f, NeuronType.Internal), new Neuron(2, 9f, NeuronType.Internal), 2f),
            new NeuronConnection(new Neuron(2, 9f, NeuronType.Internal), new ActionNeuron(2, 9f), 2f)
        });

        //Act
        var brain = new Brain(genome);

        //Assert
        brain.BrainGraph.Vertices.Should().Contain(new Neuron(2, 9f, NeuronType.Memory));
    }
}
