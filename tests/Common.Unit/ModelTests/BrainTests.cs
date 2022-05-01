using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class BrainTests {
	[Fact]
	public void Brain_ShouldPruneNeuronConnections_WhenThereAreNoValidConnections() {
		//Arrange
		var genome = new Genome(new[] { new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f) });
		
		//Act
		var brain = new Brain(genome);
		
		//Assert
		brain.SortedConnections.Should().BeEmpty();
	}
	
	[Fact]
	public void Brain_ShouldHaveOneUsedConnection_WhenThereIsInternalToOutput() {
		//Arrange
		var genome = new Genome(new[] { new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f) });
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(1);
	}
	
	[Fact]
	public void Brain_ShouldHaveNoLayer_WhenThereIsNoUsedConnection() {
		//Arrange
		var genome = new Genome(new[] { new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f) });
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(0);
	}
	
	[Fact]
	public void Brain_ShouldHaveSortedConnections_WhenThereIsAnInternal() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f)
		});
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(2);
		brain.SortedConnections[0].Should().BeEquivalentTo(new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f));
		brain.SortedConnections[1].Should().BeEquivalentTo(new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f));
	}
	
	[Fact]
	public void Brain_ShouldHave4SortedNeurons_WhenThereAre2SeparatePaths() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Input), new Neuron(3, NeuronType.Output), 2f)
		});
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedNeurons.Should().HaveCount(4);
	}
	
	[Fact]
	public void Brain_ShouldHave2SortedPaths_WhenThereAre2SeparatePaths() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Input), new Neuron(3, NeuronType.Output), 2f)
		});
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(2);
	}

	[Fact]
	public void Brain_ShouldHave2SortedPaths_WhenThereIsSplittingPath() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(3, NeuronType.Output), 2f)
		});
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(2);
	}
	
	[Fact]
	public void Brain_ShouldHave3Neurons_WhenThereIsSplittingPath() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(3, NeuronType.Output), 2f)
		});
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedNeurons.Should().HaveCount(3);		
	}
	
	[Fact]
	public void Brain_ShouldSortConnections_WhenReceivingTree() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(1, NeuronType.Output), 2f)
		});
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(3);
		brain.SortedConnections[0].Should().Be(genome.NeuronConnections[0]);
		brain.SortedConnections[1].Should().Be(genome.NeuronConnections[1]);
		brain.SortedConnections[2].Should().Be(genome.NeuronConnections[2]);
	}
	
	[Fact]
	public void Brain_ShouldSortConnections_WhenReceivingMixedTree() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(1, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
		});
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(3);
		brain.SortedConnections[0].Should().Be(genome.NeuronConnections[1]);
		brain.SortedConnections[1].Should().Be(genome.NeuronConnections[2]);
		brain.SortedConnections[2].Should().Be(genome.NeuronConnections[0]);
	}
	

	[Fact]
	public void Brain_ShouldNotPrune_WhenWeHaveInputTwoNeurons() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(3, NeuronType.Output), 2f),
		});
		
		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(3);
	}
	
	[Fact]
	public void Brain_ShouldPrune_WhenDependencyLoopOccurs() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
		});

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(4);
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f));
	}
	
	[Fact]
	public void Brain_ShouldAddMemory_WhenDependencyLoopOccurs() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
		});

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(4);
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(3, NeuronType.Memory), new Neuron(2, NeuronType.Internal), 2f));
	}

	[Fact]
	public void Brain_ShouldRemoveCorrectConnection_WhenComplexDependencyLoop() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), 2f),
		};
		
		var genome = new Genome(connections);

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(5);
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(3, NeuronType.Memory), new Neuron(4, NeuronType.Internal), 2f));
	}
	
	[Fact]
	public void Brain_ShouldRemoveCorrectConnection_WhenComplexDependencyLoopInDifferentOrder() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
		};
		
		var genome = new Genome(connections);

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(5);
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(3, NeuronType.Memory), new Neuron(4, NeuronType.Internal), 2f));
	}

	[Fact]
	public void Brain_ShouldPruneAll_WhenComplexDependencyLoop() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new Neuron(1, NeuronType.Input), new Neuron(1, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(1, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 1f),
			new NeuronConnection(new Neuron(1, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 3f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 02f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(5, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(1, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(2, NeuronType.Output), 213f),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
		};

		var genome = new Genome(connections);

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(13);
		// brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f));
		// this makes the most sense, but currently it's not the case. 
		// we could try and add something to see if it's still connected to an input. 
		
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(1, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 1f));
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f));
		
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(3, NeuronType.Memory), new Neuron(1, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(1, NeuronType.Memory), new Neuron(2, NeuronType.Internal), 1f));
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(4, NeuronType.Memory), new Neuron(1, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(5, NeuronType.Memory), new Neuron(3, NeuronType.Internal), 2f));
	}
	
	[Fact]
	public void Brain_ShouldPruneAll_WhenMultipleDependencyLoop() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new Neuron(1, NeuronType.Input), new Neuron(1, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(1, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(1, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(5, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(1, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
		};

		var genome = new Genome(connections);

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(12);
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(5, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().NotContain(new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(1, NeuronType.Internal), 2f));
		
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(3, NeuronType.Memory), new Neuron(1, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(5, NeuronType.Memory), new Neuron(3, NeuronType.Internal), 2f));
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(4, NeuronType.Memory), new Neuron(1, NeuronType.Internal), 2f));
	}

	[Fact]
	public void Brain_ShouldKeepOutputs_WhenPruning() {
		//Arrange
		var connections = new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), 2f),
		};
		
		var genome = new Genome(connections);

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
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(3, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(3, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(4, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f),
			new NeuronConnection(new Neuron(3, NeuronType.Internal), new Neuron(4, NeuronType.Internal), 2f),
		};
		
		var genome = new Genome(connections);

		//Act
		var brain = new Brain(genome);
		
		//Assert
		brain.SortedNeurons.Should().Contain(connections[0].Source);
	}


	[Fact]
	public void Brain_ShouldHaveOneUsedConnection_WhenThereIsInputToOutput() {
		//Arrange
		var genome = new Genome(new[] { new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Output), 2f) });

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(1);
	}

	[Fact]
	public void Brain_ShouldHaveTwoUsedConnection_WhenThereAreTwoConnections() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f)
		});

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(2);
	}
	
	[Fact]
	public void Brain_ShouldHaveThreeUsedConnection_WhenInternalIsSelfReferencing() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f)
		});

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().HaveCount(3);
	}
	
	[Fact]
	public void Brain_ShouldAddMemory_WhenSelfReferencing() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f)
		});

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.BrainGraph.Vertices.Should().Contain(new Neuron(2, NeuronType.Memory));
	}
	
	[Fact]
	public void Brain_ShouldAddMemoryToSorted_WhenSelfReferencing() {
		//Arrange
		var genome = new Genome(new[] {
			new NeuronConnection(new Neuron(2, NeuronType.Input), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Internal), 2f),
			new NeuronConnection(new Neuron(2, NeuronType.Internal), new Neuron(2, NeuronType.Output), 2f)
		});

		//Act
		var brain = new Brain(genome);

		//Assert
		brain.SortedConnections.Should().Contain(new NeuronConnection(new Neuron(2, NeuronType.Memory), new Neuron(2, NeuronType.Internal), 2f));
	}
}
