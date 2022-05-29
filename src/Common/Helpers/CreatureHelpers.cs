using Common.Models;
using Common.Models.Enums;

namespace Common.Helpers;

public static class CreatureHelpers {
	public static double CalculateDistanceBetweenCreatures(this Creature creature1, Creature creature2) {
		var result = creature1.Position.CalculateDistanceBetweenPositions(creature2.Position);

		result -= creature1.Radius;
		result -= creature2.Radius;

		return Math.Max(0, result);
	}
	

	public static double CalculateAngleBetweenCreatures(this Creature creature1, Creature creature2) {
		var closestPoint = creature1.GetClosestPointWithinRadius(creature2);
		return closestPoint.CalculateAngleBetweenPositions(creature2.Position);
	}
	
	public static double CalculateAngleBetweenCreatures(this Creature creature1, Creature creature2, Direction direction) {
		var closestPoint = creature1.GetClosestPointWithinRadius(creature2);
		return closestPoint.CalculateAngleBetweenPositions(creature2.Position, direction);
	}

	private static Vector GetClosestPointWithinRadius(this Creature moveTo, Creature moveFrom) {
		var moveAmount = moveFrom.Radius;

		var x = MoveTo(moveTo.Position.X, moveFrom.Position.X, moveAmount);
		var y = MoveTo(moveTo.Position.Y, moveFrom.Position.Y, moveAmount);
		
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