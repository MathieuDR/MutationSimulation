using Common.Models;

namespace Common.Helpers;

public static class LineHelpers {
	public static Vector? GetIntersectionWithinLines(this Line l1, Line l2) {
		var intersectionOfEquations = l1.GetIntersection(l2);

		// check if intersection is within the lines
		if (!intersectionOfEquations.HasValue) {
			return null;
		}

		var intersection = intersectionOfEquations.Value;
		if (l1.IsPointOnLine(intersection) && l2.IsPointOnLine(intersection)) {
			return intersection;
		}

		return null;
	}

	public static Vector? GetIntersection(this Line l1, Line l2, double tolerance = 0.001d) {
		var eq1 = l1.ToEquation();
		var eq2 = l2.ToEquation();

		var delta = eq1.A * eq2.B - eq2.A * eq1.B;
		if (Math.Abs(delta) < tolerance) {
			return null;
		}


		// check if overlap
		if (l1.Overlaps(l2, tolerance)) {
			throw new Exception("Lines overlap, intersection not possible");
		}

		var x = (eq1.B * eq2.C - eq2.B * eq1.C) / delta;
		var y = (eq2.A * eq1.C - eq1.A * eq2.C) / delta;

		return new Vector(x, y);
	}

	public static double DistanceToEquation(this Line line, Vector point) {
		var eq = line.ToEquation();
		var distance = Math.Abs(eq.A * point.X + eq.B * point.Y + eq.C) / Math.Sqrt(Math.Pow(eq.A, 2) + Math.Pow(eq.B, 2));
		return distance;
	}

	// https://stackoverflow.com/questions/849211/shortest-distance-between-a-point-and-a-line-segment
	public static double Distance(this Line line, Vector point, double tolerance = 0.001d) {
		var closestPoint = line.ClosestPoint(point, tolerance);
		return Math.Sqrt(DistanceSquared(point, closestPoint));
	}

	public static Vector ClosestPoint(this Line line, Vector point, double tolerance = 0.001d) {
		// check if the distance of the line segment is basically one point
		var l2 = DistanceSquared(line.StartPoint, line.EndPoint);
		if (l2 < tolerance) {
			return line.StartPoint;
		}

		// calculates t: ?
		var t = ((point.X - line.StartPoint.X) * (line.EndPoint.X - line.StartPoint.X) +
		         (point.Y - line.StartPoint.Y) * (line.EndPoint.Y - line.StartPoint.Y)) / l2;

		// it should be between 0 and 1
		t = Math.Max(0, Math.Min(1, t));

		// calculate the closest point
		return new Vector(line.StartPoint.X + t * (line.EndPoint.X - line.StartPoint.X),
			line.StartPoint.Y + t * (line.EndPoint.Y - line.StartPoint.Y));
	}

	private static double DistanceSquared(Vector a, Vector b) => Math.Pow(a.X - b.X, 2) + Math.Pow(a.Y - b.Y, 2);

	public static bool IsPointOnEquation(this Line line, Vector point, double tolerance = 0.001d) {
		var eq = line.ToEquation();
		return Math.Abs(eq.A * point.X + eq.B * point.Y + eq.C) < tolerance;
	}

	public static bool IsPointOnLine(this Line line, Vector point, double tolerance = 0.001d) {
		var onEq = line.IsPointOnEquation(point, tolerance);
		if (!onEq) {
			return false;
		}

		var distance = line.Distance(point);
		return distance < tolerance;
	}

	private static bool Overlaps(this Line l1, Line l2, double tolerance) {
		if (Math.Abs(l1.StartPoint.X - l1.EndPoint.X) < tolerance &&
		    Math.Abs(l2.StartPoint.X - l2.EndPoint.X) < tolerance &&
		    Math.Abs(l1.StartPoint.X - l2.StartPoint.X) < tolerance) {
			return true;
		}

		if (Math.Abs(l1.StartPoint.Y - l1.EndPoint.Y) < tolerance &&
		    Math.Abs(l2.StartPoint.Y - l2.EndPoint.Y) < tolerance &&
		    Math.Abs(l1.StartPoint.Y - l2.StartPoint.Y) < tolerance) {
			return true;
		}

		return false;
	}

	private static (double A, double B, double C) ToEquation(this Line l) {
		var a = l.StartPoint.Y - l.EndPoint.Y;
		var b = l.EndPoint.X - l.StartPoint.X;
		var c = l.StartPoint.X * l.EndPoint.Y - l.EndPoint.X * l.StartPoint.Y;
		return (a, b, c);
	}

	private static (double m, double b) ToSlopeEquation(this (double A, double B, double C) eq) {
		var m = eq.B / eq.A;
		var b = eq.C / eq.A;
		return (m, b);
	}

	private static bool IsParallelTo(this (double m, double b) l1, (double m, double b) l2, double tolerance) => Math.Abs(l1.m - l2.m) < tolerance;
}
