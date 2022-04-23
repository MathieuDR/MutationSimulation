using Common.Models;

namespace Common.Simulator; 

public static class SimulationMachine {
	public static World Tick(World world) {
		return world with { Blobs = Simulate(world), Tick = world.Tick + 1 };
	}

	private static PhysicBlob[] Simulate(World world) {
		var blobs = new PhysicBlob[world.Blobs.Length];
		
		for (var i = 0; i < world.Blobs.Length; i++) {
			var blob = world.Blobs[i];
			blobs[i] = CalculatePosition(blob, world);
		}

		return blobs;
	}

	private static PhysicBlob CalculatePosition(PhysicBlob physicBlob, World world) {
		physicBlob = CalculateVelocity(physicBlob, world);
		physicBlob = physicBlob with { Position = CalculatePositionFromVelocity(physicBlob.Position, physicBlob.Velocity) };

		return physicBlob;
	}

	private static PhysicBlob CalculateVelocity(PhysicBlob physicBlob, World world) {
		physicBlob = CalculateVelocityFromBlobCollisions(physicBlob, world.Blobs);
		physicBlob = CalculateVelocityFromWallCollisions(physicBlob, world);
		return physicBlob;
	}

	private static PhysicBlob CalculateVelocityFromWallCollisions(PhysicBlob physicBlob, World world) {
		// if we hit a wall, invert the velocity
		if (physicBlob.Position.X - physicBlob.Radius <= 0 || physicBlob.Position.X + physicBlob.Radius >= world.Width) {
			physicBlob = physicBlob with { Velocity = physicBlob.Velocity with {X =  -physicBlob.Velocity.X } };
		}
		
		if (physicBlob.Position.Y - physicBlob.Radius<= 0 || physicBlob.Position.Y + physicBlob.Radius>= world.Height) {
			physicBlob = physicBlob with { Velocity = physicBlob.Velocity with {Y =  -physicBlob.Velocity.Y } };
		}

		return physicBlob;
	}

	private static PhysicBlob CalculateVelocityFromBlobCollisions(PhysicBlob physicBlob, PhysicBlob[] blobs) {
		foreach (var otherBlob in blobs) {
			if (physicBlob == otherBlob) continue;

			// check if blobs are colliding
			// taking into account the diameter
			var distance = CalculateDistance(physicBlob, otherBlob);
			if (distance < physicBlob.Radius + otherBlob.Radius) {
				// collision detected
				// calculate the new velocity based on collision
				physicBlob = physicBlob with { Velocity = CalculateNewVelocity(physicBlob, otherBlob) };
			}
		}

		return physicBlob;
	}

	private static Velocity CalculateNewVelocity(PhysicBlob physicBlob, PhysicBlob otherPhysicBlob) {
		// calculate the new velocity based on collision
		return physicBlob.Velocity with {
			X = CalculateVelocity(physicBlob.Diameter, otherPhysicBlob.Diameter, physicBlob.Velocity.X, otherPhysicBlob.Velocity.X),
			Y = CalculateVelocity(physicBlob.Diameter, otherPhysicBlob.Diameter, physicBlob.Velocity.Y, otherPhysicBlob.Velocity.Y)
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
	
	private static double CalculateDistance(PhysicBlob physicBlob, PhysicBlob otherPhysicBlob) {
		return Math.Sqrt(Math.Pow(physicBlob.Position.X - otherPhysicBlob.Position.X, 2) + Math.Pow(physicBlob.Position.Y - otherPhysicBlob.Position.Y, 2));
	}
}
