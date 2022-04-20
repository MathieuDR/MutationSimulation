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
		if (blob.Position.X < 0 || blob.Position.X > world.Width) {
			blob = blob with { Velocity = blob.Velocity with {X =  -blob.Velocity.X } };
		}
		
		if (blob.Position.Y < 0 || blob.Position.Y > world.Height) {
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
			if (distance < blob.Diameter + otherBlob.Diameter) {
				// collision detected
				// calculate the new velocity based on collision
				blob = blob with { Velocity = CalculateNewVelocity(blob, otherBlob) };
			}
		}

		return blob;
	}

	private static Velocity CalculateNewVelocity(Blob blob, Blob otherBlob) {
		return blob.Velocity with {
			X = blob.Velocity.X - otherBlob.Velocity.X,
			Y = blob.Velocity.Y - otherBlob.Velocity.Y
		};
	}

	private static Position CalculatePositionFromVelocity(Position position, Velocity velocity) {
		return position with { X = position.X + velocity.X, Y = position.Y + velocity.Y };
	}
	
	private static double CalculateDistance(Blob blob, Blob otherBlob) {
		return Math.Sqrt(Math.Pow(blob.Position.X - otherBlob.Position.X, 2) + Math.Pow(blob.Position.Y - otherBlob.Position.Y, 2));
	}
}
