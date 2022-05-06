using Common.Models;

namespace Common.Helpers;

public static class CreatureHelpers {
	public static double CalculateDistanceBetweenCreatures(this Creature creature1, Creature creature2) {
		var a = creature1.Position.X - creature2.Position.X;
		var b = creature1.Position.Y - creature2.Position.Y;
		var c = Math.Pow(a, 2) + Math.Pow(b, 2);

		var result = Math.Sqrt(c);

		result -= creature1.Radius;
		result -= creature2.Radius;

		return Math.Max(0, result);
	}
	
	public static double CalculateAngleBetweenCreatures(this Creature creature1, Creature creature2) {
		var radianAngle = Math.Atan2(creature2.Position.Y - creature1.Position.Y, creature2.Position.X - creature1.Position.X);
		
		if (radianAngle < 0) {
			radianAngle += 2 * Math.PI;
		}
		
		return radianAngle * 180 / Math.PI;
	}
	
	public static double CalculateAngleBetweenCreatures(this Creature creature1, Creature creature2, Direction direction) {
		var angle = creature1.CalculateAngleBetweenCreatures(creature2);

		angle -= direction switch {
			Direction.North => 90,
			Direction.East => 0,
			Direction.South => 270,
			Direction.West => 180,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
		
		if(angle < 0) {
			angle += 360;
		}

		return angle;
	}
}
