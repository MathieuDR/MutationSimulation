using Common.Helpers;
using Common.Models.Genetic;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using Common.Simulator;
using MathieuDR.Common.Extensions;
using SkiaSharp;

namespace Common.Models;

public enum Direction {
	North,
	East,
	South,
	West,
}


public record Creature {
	private readonly Genome _genome;
	public int Age { get;init; } = 0;

	public int EyeSightStrength => Radius * 5;

	public Brain Brain { get; private set; }
	
	private Dictionary<Neuron, float> _neuronValues = new Dictionary<Neuron, float>();
	public Creature(Genome Genome, Position Position, int Radius, Color Color) {
		this.Position = Position;
		this.Radius = Radius;
		this.Color = Color;
		this.Genome = Genome;
	}

	public Direction Direction { get; init; } = RandomProvider.GetRandom().NextEnum<Direction>();

	public Genome Genome {
		get => _genome;
		init {
			_genome = value;
			Brain = new Brain(_genome);
		}
	}

	public Position Position { get; init; }
	public int Radius { get; init; }
	public Color Color { get; init; }

	public Creature Simulate(World world) {
		
		foreach (var neuron in Brain.SortedNeurons) {
			switch (neuron) {
				case InputNeuron inputNeuron:
					// do input stuff
					var input = GetInput(inputNeuron, world);
					_neuronValues.Add(neuron, input);
					break;
				case ActionNeuron actionNeuron:
					// do action stuff
					var action = DoAction(actionNeuron, world);
					break;
				case Neuron internalNeuron:
					var value = GetValue(internalNeuron);
					// do neuron stuff
					break;
			}

		}
		
		return this with {Age = Age + 1};
	}

	private object GetValue(Neuron internalNeuron) {
		throw new NotImplementedException();
	}

	private object DoAction(ActionNeuron actionNeuron, World world) {
		throw new NotImplementedException();
	}

	private float GetInput(InputNeuron inputNeuron, World world) {
		return inputNeuron.InputTypeType switch {
			InputType.LookFront => Look(world, Direction),
			InputType.LookLeft =>  Look(world, (Direction)((int)(Direction - 1) % 4)),
			InputType.LookRight => Look(world, (Direction)((int)(Direction + 1) % 4)),
			InputType.LookBack => Look(world, (Direction)((int)(Direction + 2) % 4)),
			InputType.SmellFood => Smell(world, InputType.SmellFood),
			InputType.SmellPheromone => Smell(world, InputType.SmellPheromone),
			InputType.Oscillator => Oscillator(),
			InputType.Hunger => 0,
			InputType.Tiredness => 0,
			InputType.PheromoneStrength => 0,
			InputType.PheromoneDecay => 0,
			InputType.IsEmittingPheromone => 0,
			InputType.Age => Age,
			InputType.Speed => 1,
			_ => 0
		};
	}

	private float Oscillator() {
		throw new NotImplementedException();
	}

	private float Smell(World world, InputType smellFood) {
		throw new NotImplementedException();
	}

	private float Look(World world, Direction direction) {
		float bestValue = 1;
		
		// check which is in the fov of the creature
		foreach (var worldCreature in world.Creatures) {
			if(this == worldCreature) continue;

			var distance = this.CalculateDistanceBetweenCreatures(worldCreature);

			if (distance > EyeSightStrength) {
				// we cannot see
				continue;
			}
			
			var angle = this.CalculateAngleBetweenCreatures(worldCreature, direction);

			if (angle >= 90) {
				continue; // ?
			}
			
			var strength = 1 - (float)(distance / EyeSightStrength);
			bestValue = Math.Min(strength, bestValue);
		}

		return bestValue;
	}

	public void Draw(SKCanvas canvas, Func<Position, (int X, int Y)> calculatePixelPosition, Func<int, int> pixelSize) {
		var fillPaint = new SKPaint {
			Style = SKPaintStyle.Fill,
			Color = new SKColor(Color.R, Color.G, Color.B)
		};
		
		var pixelPosition = calculatePixelPosition(Position);

		canvas.DrawCircle(pixelPosition.X, pixelPosition.Y , pixelSize(Radius) , fillPaint);
	}

	public void Deconstruct(out Genome Genome, out Position Position, out int Radius, out Color Color) {
		Genome = this.Genome;
		Position = this.Position;
		Radius = this.Radius;
		Color = this.Color;
	}
}
