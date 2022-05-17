using Common.Models;

namespace Common.Helpers; 

public static class PositionHelper {
	public static double CalculateDistanceBetweenPositions(this Vector vectorA, Vector vectorB) {
		var a = vectorA.X - vectorB.X;
		var b = vectorA.Y - vectorB.Y;
		var c = Math.Pow(a, 2) + Math.Pow(b, 2);

		return Math.Sqrt(c);
	}

	public static double CalculateAngleBetweenPositions(this Vector vectorA, Vector vectorB) {
		var radianAngle = Math.Atan2(vectorB.Y - vectorA.Y, vectorB.X - vectorA.X);
		
		if (radianAngle < 0) {
			radianAngle += 2 * Math.PI;
		}
		
		return radianAngle * 180 / Math.PI;
	}
	
	public static double CalculateAngleBetweenPositions(this Vector vectorA, Vector vectorB, Direction direction) {
		var angle = vectorA.CalculateAngleBetweenPositions(vectorB);
		return ChangeDefaultAngleToDirection(angle,direction);
	}

	public static double ChangeDefaultAngleToDirection(double angle, Direction direction) {
		angle -= direction switch {
			Direction.North => 90,
			Direction.East => 180,
			Direction.South => 270,
			Direction.West => 0,
			_ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
		};
		
		if(angle < 0) {
			angle += 360;
		}
		
		return angle;
	}

	public static bool IsInViewingAngle(double angle, double viewingAngle) {
		var minAngle = viewingAngle / 2;
		return angle <= minAngle || angle >= 360 - minAngle;
	}
}
