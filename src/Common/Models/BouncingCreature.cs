
using Common.Interfaces;
using SkiaSharp;

namespace Common.Models; 


public record BouncingCreature(Position Position, Velocity Velocity, int Diameter, Color Color) : ICreature {
	public BouncingCreature(Random random, int maxVelocity, int maxDiameter, int worldWidth, int worldHeight) : 
		this( new(0, 0), new Velocity(random, maxVelocity), random.Next(5, Math.Max(6, maxDiameter)), new Color(random)) {
		Position = new Position(random, worldWidth, worldHeight, Diameter);
	}

	public int Radius => Diameter / 2;
	
	public ICreature Simulate(World world) {
		return CalculatePosition(this, world);
	}

	public void Draw(SKCanvas canvas, Func<Position, (int X, int Y)> calculatePixelPosition, Func<int, int> pixelSize) {
		var fillPaint = new SKPaint {
			Style = SKPaintStyle.Fill,
			Color = new SKColor(Color.R, Color.G, Color.B)
		};
		
		var pixelPosition = calculatePixelPosition(Position);

		canvas.DrawCircle(pixelPosition.X, pixelPosition.Y , pixelSize(Radius) , fillPaint);
	}
	
	private static BouncingCreature CalculatePosition(BouncingCreature bouncingCreature, World world) {
		bouncingCreature = CalculateVelocity(bouncingCreature, world);
		bouncingCreature = bouncingCreature with { Position = CalculatePositionFromVelocity(bouncingCreature.Position, bouncingCreature.Velocity) };

		return bouncingCreature;
	}

	private static BouncingCreature CalculateVelocity(BouncingCreature bouncingCreature, World world) {
		var otherBouncers = world.Blobs.Where(b=> b is BouncingCreature).Cast<BouncingCreature>();
		bouncingCreature = CalculateVelocityFromBlobCollisions(bouncingCreature, otherBouncers);
		bouncingCreature = CalculateVelocityFromWallCollisions(bouncingCreature, world);
		return bouncingCreature;
	}

	private static BouncingCreature CalculateVelocityFromWallCollisions(BouncingCreature bouncingCreature, World world) {
		// if we hit a wall, invert the velocity
		if (bouncingCreature.Position.X - bouncingCreature.Radius <= 0 || bouncingCreature.Position.X + bouncingCreature.Radius >= world.Width) {
			bouncingCreature = bouncingCreature with { Velocity = bouncingCreature.Velocity with {X =  -bouncingCreature.Velocity.X } };
		}
		
		if (bouncingCreature.Position.Y - bouncingCreature.Radius<= 0 || bouncingCreature.Position.Y + bouncingCreature.Radius>= world.Height) {
			bouncingCreature = bouncingCreature with { Velocity = bouncingCreature.Velocity with {Y =  -bouncingCreature.Velocity.Y } };
		}

		return bouncingCreature;
	}

	private static BouncingCreature CalculateVelocityFromBlobCollisions(BouncingCreature bouncingCreature, IEnumerable<BouncingCreature> blobs) {
		foreach (var otherBlob in blobs) {
			if (bouncingCreature == otherBlob) continue;

			// check if blobs are colliding
			// taking into account the diameter
			var distance = CalculateDistance(bouncingCreature, otherBlob);
			if (distance < bouncingCreature.Radius + otherBlob.Radius) {
				// collision detected
				// calculate the new velocity based on collision
				bouncingCreature = bouncingCreature with { Velocity = CalculateNewVelocity(bouncingCreature, otherBlob) };
			}
		}

		return bouncingCreature;
	}

	private static Velocity CalculateNewVelocity(BouncingCreature bouncingCreature, BouncingCreature otherBouncingCreature) {
		// calculate the new velocity based on collision
		return bouncingCreature.Velocity with {
			X = CalculateVelocity(bouncingCreature.Diameter, otherBouncingCreature.Diameter, bouncingCreature.Velocity.X, otherBouncingCreature.Velocity.X),
			Y = CalculateVelocity(bouncingCreature.Diameter, otherBouncingCreature.Diameter, bouncingCreature.Velocity.Y, otherBouncingCreature.Velocity.Y)
		};
	}

	private static double CalculateVelocity(int d1, int d2, double v1, double v2) {
		return v1 * -1;
	}

	private static int CalculateMass(int d1, int dMult = 10) {
		return d1*dMult;
	}
	
	private static Position CalculatePositionFromVelocity(Position position, Velocity velocity) {
		return position with { X = position.X + (int)Math.Round(velocity.X, 0), Y = position.Y + (int)Math.Round(velocity.Y, 0) };
	}
	
	private static double CalculateDistance(BouncingCreature bouncingCreature, BouncingCreature otherBouncingCreature) {
		return Math.Sqrt(Math.Pow(bouncingCreature.Position.X - otherBouncingCreature.Position.X, 2) + Math.Pow(bouncingCreature.Position.Y - otherBouncingCreature.Position.Y, 2));
	}
}

public record Velocity(double X, double Y) {
	public Velocity(Random random, int maxVelocity) : this((random.NextDouble() - random.NextDouble()) * maxVelocity, (random.NextDouble() - random.NextDouble())  * maxVelocity){ }
};
