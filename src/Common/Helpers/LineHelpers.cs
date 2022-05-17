using Common.Models;

namespace Common.Helpers; 

public static class LineHelpers {
	public static Vector? GetIntersectionWithinLines(this Line l1, Line l2, double? tolerance = null) {
		var toleranceValue = tolerance ?? 0.001d;
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
	
	public static Vector? GetIntersection(this Line l1, Line l2, double? tolerance = null) {
		var toleranceValue = tolerance ?? 0.001d;
		var eq1 = l1.ToEquation();
		var eq2 = l2.ToEquation();
		
		var delta = eq1.A * eq2.B - eq2.A * eq1.B;
		if(Math.Abs(delta) < toleranceValue) {
			return null;
		}
		
		
		// check if overlap
		if(l1.Overlaps(l2, toleranceValue)) {
			throw new Exception("Lines overlap, intersection not possible");
		}
		
		var x = (eq2.B * eq1.C - eq1.B * eq2.C) / delta;
		var y = (eq1.A * eq2.C - eq2.A * eq1.C) / delta;
		
		return new Vector(x, y);
	}

	public static double DistanceToLine(this Line line, Vector point) {
		var eq = line.ToEquation();
		var distance = Math.Abs(eq.A * point.X + eq.B * point.Y + eq.C) / Math.Sqrt(Math.Pow(eq.A, 2) + Math.Pow(eq.B, 2));
		return distance;
		
	}
	
	public static bool IsPointOnLine(this Line line, Vector point, double? tolerance = null) {
		var toleranceValue = tolerance ?? 0.001d;
		var eq = line.ToEquation();
		return Math.Abs((eq.A * point.X) + (eq.B * point.Y) + eq.C) < toleranceValue;
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
		var a = l.EndPoint.Y - l.StartPoint.Y;
		var b = l.StartPoint.X - l.EndPoint.X;
		var c = a * l.StartPoint.X + b * l.StartPoint.Y;
		return (a, b, c);
	}
	
	private static (double m, double b) ToSlopeEquation(this (double A, double B, double C) eq) {
		var m = eq.B / eq.A;
		var b = eq.C / eq.A;
		return (m, b);
	}

	private static bool IsParallelTo(this (double m, double b) l1, (double m, double b) l2, double tolerance) {
		return Math.Abs(l1.m - l2.m) < tolerance;
	}
}
