using Common.Models;

namespace Common.Simulator; 

public static class SimulationMachine {
	public static World Tick(World world) {
		return world with { Blobs = Simulate(world), Tick = world.Tick + 1 };
	}

	private static Blob[] Simulate(World world) {
		var blobs = new Blob[world.Blobs.Length];
		
		for (var i = 0; i < world.Blobs.Length; i++) {
			var blob = world.Blobs[i];
			blobs[i] = CalculatePosition(blob, world);
		}

		return blobs;
	}

	private static Blob CalculatePosition(Blob blob, World world) {
		blob = CalculateVelocity(blob, world);
		blob = blob with { Position = CalculatePositionFromVelocity(blob.Position, blob.Velocity) };

		return blob;
	}

	private static Blob CalculateVelocity(Blob blob, World world) {
		blob = CalculateVelocityFromBlobCollisions(blob, world.Blobs);
		blob = CalculateVelocityFromWallCollisions(blob, world);
		return blob;
	}

	private static Blob CalculateVelocityFromWallCollisions(Blob blob, World world) {
		// if we hit a wall, invert the velocity
		if (blob.Position.X - blob.Radius <= 0 || blob.Position.X + blob.Radius >= world.Width) {
			blob = blob with { Velocity = blob.Velocity with {X =  -blob.Velocity.X } };
		}
		
		if (blob.Position.Y - blob.Radius<= 0 || blob.Position.Y + blob.Radius>= world.Height) {
			blob = blob with { Velocity = blob.Velocity with {Y =  -blob.Velocity.Y } };
		}

		return blob;
	}

	private static Blob CalculateVelocityFromBlobCollisions(Blob blob, Blob[] blobs) {
		foreach (var otherBlob in blobs) {
			if (blob == otherBlob) continue;

			// check if blobs are colliding
			// taking into account the diameter
			var distance = CalculateDistance(blob, otherBlob);
			if (distance < blob.Radius + otherBlob.Radius) {
				// collision detected
				// calculate the new velocity based on collision
				blob = blob with { Velocity = CalculateNewVelocity(blob, otherBlob) };
			}
		}

		return blob;
	}

	private static Velocity CalculateNewVelocity(Blob blob, Blob otherBlob) {
		// calculate the new velocity based on collision
		return blob.Velocity with {
			X = CalculateVelocity(blob.Diameter, otherBlob.Diameter, blob.Velocity.X, otherBlob.Velocity.X),
			Y = CalculateVelocity(blob.Diameter, otherBlob.Diameter, blob.Velocity.Y, otherBlob.Velocity.Y)
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
	
	private static double CalculateDistance(Blob blob, Blob otherBlob) {
		return Math.Sqrt(Math.Pow(blob.Position.X - otherBlob.Position.X, 2) + Math.Pow(blob.Position.Y - otherBlob.Position.Y, 2));
	}
}
