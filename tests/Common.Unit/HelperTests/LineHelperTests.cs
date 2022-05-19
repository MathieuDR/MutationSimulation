using System;
using Common.Helpers;
using Common.Models;
using FluentAssertions;
using Xunit;

namespace Common.Unit.HelperTests; 

public class LineHelperTests {
	[Theory]
	[InlineData(5, 0, 5, 20, 5, 10, "Vertical Line")]
	[InlineData(5, 0, 25, 0, 15, 0, "Horizontal Line")]
	[InlineData(5, 0, 10, 5, 7, 2, "Diagonal")]
	public void IsPointOnLine_ShouldReturnTrue_WhenOnLine(int x1, int y1, int x2, int y2, int x3, int y3, string info) {
		//Arrange
		var line = new Line(new Vector(x1, y1), new Vector(x2, y2));
		var point = new Vector(x3, y3);
		
		//Act
		var result = line.IsPointOnLine(point);

		//Assert
		result.Should().BeTrue(info);
	}
	
	[Theory]
	[InlineData(5, 0, 5, 20, 8, 10, "Vertical Line")]
	[InlineData(5, 0, 25, 0, 4, 0, "Horizontal Line")]
	[InlineData(5, 0, 10, 5, 8, 2, "Diagonal")]
	public void IsPointOnLine_ShouldReturnFalse_WhenNotOnLine(int x1, int y1, int x2, int y2, int x3, int y3, string info) {
		//Arrange
		var line = new Line(new Vector(x1, y1), new Vector(x2, y2));
		var point = new Vector(x3, y3);
		
		//Act
		var result = line.IsPointOnLine(point);

		//Assert
		result.Should().BeFalse(info);
	}

	[Fact]
	public void IsPointOnLine_ShouldReturnFalse_WhenOnDiagonalEquationButNotInBounds() {
		//Arrange
		var line = new Line(new Vector(10, 5), new Vector(20, 15));
		var point = new Vector(30, 25);

		//Act
		var result = line.IsPointOnLine(point);
		
		//Assert
		result.Should().BeFalse();
	}
	
	[Fact]
	public void IsPointOnEquation_ShouldReturnTrue_WhenOnDiagonalEquationButNotInBounds() {
		//Arrange
		var line = new Line(new Vector(10, 5), new Vector(20, 15));
		var point = new Vector(30, 25);

		//Act
		var result = line.IsPointOnEquation(point);
		
		//Assert
		result.Should().BeTrue();
	}

	[Theory]
	[InlineData(5, 0, 5, 20, 4, 20, 1, "One X less of vertical line")]
	[InlineData(5, 0, 5, 20, 6, 20, 1, "One X more of vertical line")]
	[InlineData(5, 0, 5, 20, 6, 12, 1, "One X more of vertical line in the middle")]
	[InlineData(5, 0, 5, 20, 20, 12, 15, "15 X more of vertical line in the middle")]
	[InlineData(5, 0, 5, 20, 0, 12, 5, "5 X more of vertical line in the middle")]
	[InlineData(5, 0, 5, 20, 5, 21, 1, "One Y more of vertical line")]
	[InlineData(5, 5, 5, 20, 5, 4, 1, "One Y less of vertical line")]
	[InlineData(5, 0, 5, 20, 5, 120, 100, "100 Y more of vertical line")]
	[InlineData(50, 0, 100, 0, 49, 0, 1, "One X less of horizontal line")]
	[InlineData(50, 0, 100, 0, 101, 0, 1, "One X more of horizontal line")]
	[InlineData(50, 0, 100, 0, 75, 1, 1, "One Y more of horizontal line in the middle")]
	[InlineData(50, 0, 100, 0, 75, 15, 15, "15 Y more of horizontal line in the middle")]
	[InlineData(50, 0, 100, 0, 75, 5, 5, "5 Y more of horizontal line in the middle")]
	[InlineData(50, 0, 100, 0, 75, -5, 5, "5 Y less of horizontal line in the middle, in negative")]
	[InlineData(50, 0, 100, 0, 75, 1, 1, "One Y more of horizontal line")]
	[InlineData(50, 0, 100, 0, 75, -1, 1, "One Y less of horizontal line, in negative")]
	[InlineData(50, 0, 100, 0, 75, 100, 100, "100 Y more of horizontal line")]
	public void DistanceToLine_ShouldBeCorrect_WhenGivenInput(int x1, int y1, int x2, int y2, int x3, int y3, double expected, string info) {
		//Arrange
		var line = new Line(new Vector(x1, y1), new Vector(x2, y2));
		var point = new Vector(x3, y3);
		
		//Act
		var result = line.Distance(point);

		//Assert
		result.Should().BeApproximately(expected, 0.001, info);
	}

	[Fact]
	public void GetIntersectionWithinLines_ShouldReturnNull_WhenNoIntersection() {
		//Arrange
		var l1 = new Line(new Vector(10, 10), new Vector(20, 20));
		var l2 = new Line(new Vector(0, 9), new Vector(9, 0));
		
		//Act
		var result = l1.GetIntersectionWithinLines(l2);

		//Assert
		result.Should().BeNull();
	}
	
	[Fact]
	public void GetIntersectionWithinLines_ShouldReturnNull_WhenParalell() {
		//Arrange
		var l1 = new Line(new Vector(10, 10), new Vector(20, 20));
		var l2 = new Line(new Vector(12, 12), new Vector(22, 22));
		
		//Act
		var result = l1.GetIntersectionWithinLines(l2);

		//Assert
		result.Should().BeNull();
	}
	
	[Fact]
	public void GetIntersectionWithinLines_ShouldReturnPoint_WhenCrossing() {
		//Arrange
		var l1 = new Line(new Vector(10, 10), new Vector(20, 10));
		var l2 = new Line(new Vector(15, 0), new Vector(15, 20));
		
		//Act
		var result = l1.GetIntersectionWithinLines(l2);

		//Assert
		result.Should().Be(new Vector(15, 10));
	}
	
	[Fact]
	public void GetIntersectionWithinLines_ShouldReturnPoint_WhenCrossingAndInNegative() {
		//Arrange
		var l1 = new Line(new Vector(-10, -10), new Vector(-20, -10));
		var l2 = new Line(new Vector(-15, 0), new Vector(-15, -20));
		
		//Act
		var result = l1.GetIntersectionWithinLines(l2);

		//Assert
		result.Should().Be(new Vector(-15, -10));
	}
}
