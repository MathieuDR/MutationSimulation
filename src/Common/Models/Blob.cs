namespace Common.Models; 

public record Blob {
	public Blob(Position Position, Velocity Velocity, int Diameter) {
		this.Position = Position;
		this.Velocity = Velocity;
		this.Diameter = Diameter;
	}
	public Position Position { get; init; }
	public Velocity Velocity { get; init; }
	public int Diameter { get; init; }
	public int Radius => Diameter / 2;
	
	public void Deconstruct(out Position Position, out Velocity Velocity, out int Diameter) {
		Position = this.Position;
		Velocity = this.Velocity;
		Diameter = this.Diameter;
	}
}

public record Position(int X, int Y);
public record Velocity(int X, int Y);