using System.Runtime.InteropServices;
using Common.Models;

namespace Common.Helpers;

public static class CreatureHelpers {
	public static double CalculateDistanceBetweenCreatures(this Creature creature1, Creature creature2) {
		var result = creature1.Vector.CalculateDistanceBetweenPositions(creature2.Vector);

		result -= creature1.Radius;
		result -= creature2.Radius;

		return Math.Max(0, result);
	}
	

	public static double CalculateAngleBetweenCreatures(this Creature creature1, Creature creature2) {
		var closestPoint = creature1.GetClosestPointWithinRadius(creature2);
		return closestPoint.CalculateAngleBetweenPositions(creature2.Vector);
	}
	
	public static double CalculateAngleBetweenCreatures(this Creature creature1, Creature creature2, Direction direction) {
		var closestPoint = creature1.GetClosestPointWithinRadius(creature2);
		return closestPoint.CalculateAngleBetweenPositions(creature2.Vector, direction);
	}

	private static Vector GetClosestPointWithinRadius(this Creature moveTo, Creature moveFrom) {
		var moveAmount = moveFrom.Radius;

		var x = MoveTo(moveTo.Vector.X, moveFrom.Vector.X, moveAmount);
		var y = MoveTo(moveTo.Vector.Y, moveFrom.Vector.Y, moveAmount);
		
		return new Vector(x, y);
	}
	

	private static double MoveTo(double moveTo, double moveFrom, double moveAmount) {
		if (Math.Abs(moveTo - moveFrom) <= moveAmount + 0.5) {
			return moveFrom;
		}
		
		if (moveTo > moveFrom) {
			return moveFrom + moveAmount;
		}
		
		return moveFrom - moveAmount;
	}
}