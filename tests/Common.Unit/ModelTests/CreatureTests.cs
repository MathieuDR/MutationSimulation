using System;
using Common.Helpers;
using Common.Models;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using FluentAssertions;
using Xunit;

namespace Common.Unit.ModelTests; 

public class CreatureTests {

	private Genome EmptyGenome() {
		return new Genome(new NeuronConnection[] {
			new NeuronConnection(new Neuron(1, NeuronType.Input), new Neuron(2, NeuronType.Action), 3f)
		});
	}
	
	[Fact]
	public void CalculateDistance_ShouldReturnDistance_WhenAreNextToEachotherHorizontal() {
		//Arrange
		var creature1 = new Creature(EmptyGenome(),new Position(20, 20), 5, new Color(20, 20, 20));
		var creature2 = new Creature(EmptyGenome(),new Position(40, 20), 5, new Color(20, 20, 20));

		//Act;
		var result = creature1.CalculateDistanceBetweenCreatures(creature2);
		

		//Assert
		result.Should().Be(10);
	}
	
	[Fact]
	public void CalculateDistance_ShouldReturnDistance_WhenAreNextToEachotherVertical() {
		//Arrange
		var creature1 = new Creature(EmptyGenome(),new Position(20, 20), 5, new Color(20, 20, 20));
		var creature2 = new Creature(EmptyGenome(),new Position(20, 40), 5, new Color(20, 20, 20));

		//Act;
		var result = creature1.CalculateDistanceBetweenCreatures(creature2);
		

		//Assert
		result.Should().Be(10);
	}
	
	[Fact]
	public void CalculateDistance_ShouldReturnDistance_WhenDiagonal() {
		//Arrange
		var creature1 = new Creature(EmptyGenome(),new Position(20, 20), 5, new Color(20, 20, 20));
		var creature2 = new Creature(EmptyGenome(),new Position(40, 40), 5, new Color(20, 20, 20));

		//Act;
		var result = creature1.CalculateDistanceBetweenCreatures(creature2);
		

		//Assert
		result.Should().Be(Math.Sqrt(20*20+20*20) - 10);
	}
	
	[Fact]
	public void CalculateDistance_ShouldReturn0Distance_WhenTheyAreTouching() {
		//Arrange
		var creature1 = new Creature(EmptyGenome(),new Position(20, 20), 5, new Color(20, 20, 20));
		var creature2 = new Creature(EmptyGenome(),new Position(20, 28), 5, new Color(20, 20, 20));

		//Act;
		var result = creature1.CalculateDistanceBetweenCreatures(creature2);
		

		//Assert
		result.Should().Be(0);
	}
	
	[Theory]
	[InlineData(0, 0, 0)]
	[InlineData(20, 0, 0)]
	[InlineData(20, 20, 45)]
	[InlineData(0, 20, 90)]
	[InlineData(-20, 0, 180)]
	[InlineData(-20, -20, 225)]
	[InlineData(0, -20, 270)]
	[InlineData(20, -20, 315)]
	[InlineData(-20, 20, 135)]
	public void CalculateAngle_ShouldBeCorrect_WhenFirstVectorIsZeroZero(int x2, int y2, double expected) {
		//Arrange
		var creature1 = new Creature(EmptyGenome(),new Position(0, 0), 5, new Color(20, 20, 20));
		var creature2 = new Creature(EmptyGenome(),new Position(x2, y2), 5, new Color(20, 20, 20));

		//Act;
		var result = creature1.CalculateAngleBetweenCreatures(creature2);
		

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0, 0, 0, 20)]
	[InlineData(20, 0, 0, 10)]
	[InlineData(20, 20, 45, -10)]
	[InlineData(0, 20, 90, -20)]
	[InlineData(-20, 0, 180, 99)]
	[InlineData(-20, -20, 225, 15)]
	[InlineData(0, -20, 270, 22)]
	[InlineData(20, -20, 315, 33)]
	[InlineData(-20, 20, 135, -42)]
	public void CalculateAngle_ShouldBeCorrect_WhenFVectorsHaveAnOffset(int x2, int y2, double expected, int offset) {
		//Arrange
		var creature1 = new Creature(EmptyGenome(),new Position(0 + offset, 0 + offset), 5, new Color(20, 20, 20));
		var creature2 = new Creature(EmptyGenome(),new Position(x2 + offset, y2 + offset), 5, new Color(20, 20, 20));

		//Act;
		var result = creature1.CalculateAngleBetweenCreatures(creature2);
		

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(0, 0, 0)]
	[InlineData(20, 0, 180)]
	[InlineData(20, 20, 225)]
	[InlineData(0, 20, 270)]
	[InlineData(-20, 0, 0)]
	[InlineData(-20, -20, 45)]
	[InlineData(0, -20, 90)]
	[InlineData(20, -20, 135)]
	[InlineData(-20, 20, 315)]
	public void CalculateAngle_ShouldBeCorrect_WhenSecondVectorIsZeroZero(int x1, int y1, double expected) {
		//Arrange
		var creature1 = new Creature(EmptyGenome(),new Position(x1, y1), 5, new Color(20, 20, 20));
		var creature2 = new Creature(EmptyGenome(),new Position(0, 0), 5, new Color(20, 20, 20));

		//Act;
		var result = creature1.CalculateAngleBetweenCreatures(creature2);

		//Assert
		result.Should().Be(expected);
	}
	
	[Theory]
	[InlineData(20, 0, Direction.East)]
	[InlineData(0, 20, Direction.North)]
	[InlineData(-20, 0, Direction.West)]
	[InlineData(0, -20, Direction.South)]
	public void CalculateAngle_ShouldBeZero_WhenGivenDirection(int x2, int y2, Direction direction) {
		//Arrange
		var creature1 = new Creature(EmptyGenome(),new Position(0, 0), 5, new Color(20, 20, 20));
		var creature2 = new Creature(EmptyGenome(),new Position(x2, y2), 5, new Color(20, 20, 20));

		//Act;
		var result = creature1.CalculateAngleBetweenCreatures(creature2, direction);
		

		//Assert
		result.Should().Be(0);
	}
}
