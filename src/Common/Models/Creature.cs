using Common.Helpers;
using Common.Models.Enums;
using Common.Models.Genetic;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using MathieuDR.Common.Extensions;
using SkiaSharp;

namespace Common.Models;

public record Creature {
	private readonly Random _random;
	private readonly Dictionary<ActionCategory, Dictionary<ActionType, float>> _actionValues;
	private readonly Genome _genome;
	private readonly Dictionary<Neuron, float> _neuronValues = new();


	private Dictionary<Creature, double> _creatureDistances = new();

	public Creature(Genome genome, Vector position, int radius, Color color, Random random) {
		_random = random;
		Id = (ulong)_random.NextInt64(long.MinValue, long.MaxValue);
		Direction = _random.NextEnum<Direction>();
		Position = position;
		Radius = radius;
		Color = color;
		Genome = genome;
		StartPosition = position;
	}

	public ulong Id { get; }

	public int Age { get; private set; }
	public float OscillatorFrequency => 5000f;
	public float OscillatorPhaseOffset => 5000f;
	public int OscillatorAgeDivider => 1000;
	public int EyeSightStrength => Radius * 4 + (int)Speed;
	public int ViewingAngle => 40;
	public float Speed => Math.Max((float)Radius / 4, 3) / 20;
	
	public Vector StartPosition { get; init; }

	public int Collisions { get; private set; } = 0;
	public bool Collided { get; private set; } = false;
	public List<Vector> UniquePositions = new();

	public Brain Brain { get; private set; }
	public double Distance { get; private set; } = 0;

	private Func<float, float> InternalActivationFunction { get; } = ActivationFunctions.Relu;

	public Direction Direction { get; private set; }

	public Genome Genome {
		get => _genome;
		init {
			_genome = value;
			Brain = new Brain(_genome);

			_actionValues = Brain.ActionNeurons
				.GroupBy(x => x.ActionType.GetActionCategory())
				.ToDictionary(x => x.Key,
					x => x.ToDictionary(y => y.ActionType, y => 0f));
		}
	}

	public Vector Position { get; private set; }
	public int Radius { get; init; }
	public Color Color { get; init; }

	private void PrepareNextTick() {
		_creatureDistances.Clear();
	}

	public void Simulate(World world) {
		PrepareNextTick();
		Age++;
		FeedForward(world);
		FireActions(world);
	}

	private void Act(World world, ActionType actionType) {
		switch (actionType) {
			case ActionType.WalkForward:
				Walk(world);
				break;
			case ActionType.WalkBackward:
				Walk(world, false);
				break;
			case ActionType.TurnLeft:
				Direction = (Direction)(((int)Direction + 1) % 4);
				break;
			case ActionType.TurnRight:
				Direction = (Direction)(((int)Direction + 3) % 4);
				break;
			case ActionType.TurnAround:
				Direction = (Direction)(((int)Direction + 2) % 4);
				break;
			case ActionType.Eat:
			case ActionType.Sleep:
			case ActionType.EmitPheromone:
			case ActionType.SetOscillator:
			case ActionType.SetPheromoneStrength:
			case ActionType.SetPheromoneDecay:
			case ActionType.SetSpeed:
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(actionType), actionType, null);
		}
	}

	private void Walk(World world, bool forward = true) {
		var xMovement = Direction switch {
			Direction.East => -Speed,
			Direction.West => Speed,
			_ => 0
		};

		var yMovement = Direction switch {
			Direction.North => -Speed,
			Direction.South => Speed,
			_ => 0
		};


		if (forward) {
			xMovement *= -1;
			yMovement *= -1;
		}

		var proposedPosition = Position with { X = Position.X + xMovement, Y = Position.Y + yMovement };
		var movementLine = new Line(this.Position, proposedPosition);

		var creaturesToCheck = _creatureDistances.Any() ? 
			_creatureDistances.Where(x => x.Value >= Speed * 2).Select(x=>x.Key).ToArray() : 
			world.Creatures;

		foreach (var creature in creaturesToCheck) {
			if (this == creature) {
				continue;
			}
			
			var dist = Math.Max(0, movementLine.Distance(creature.Position) - creature.Radius - Radius);
			if (dist <= 0.5d) {
				Collisions++;
				Collided = true;
				// TODO calculate last position before collision
				
				return;
			}
		}

		foreach (var wall in world.Walls) {
			var intersect = wall.GetIntersectionWithinLines(movementLine);
			if (intersect is not null) {
				Collisions++;
				Collided = true;
				// TODO calculate last position before collision
				return;
			}

			var endDist = Math.Max(wall.Distance(proposedPosition) - Radius, 0);
			if (endDist <= 0.5d) {
				Collisions++;
				Collided = true;
				// TODO calculate last position before collision
				return;
			}
		}

		Collided = false;
		Position = proposedPosition;
		Distance += Speed;
		if (!UniquePositions.Contains(Position)) {
			UniquePositions.Add(Position);
		}
	}

	private void FireActions(World world) {
		ActivateActionValues();

		var rng = _random.NextSingle();

		foreach (var actionCategory in _actionValues.Keys) {
			var max = _actionValues[actionCategory].Aggregate((l, r) => l.Value > r.Value ? l : r);

			if (rng < max.Value) {
				Act(world, max.Key);
			}
		}
	}

	private void ActivateActionValues() {
		foreach (var kvp in _actionValues) {
			if (kvp.Value.Count > 1) {
				_actionValues[kvp.Key] = SoftMax(kvp.Value);
			} else {
				var kvp2 = kvp.Value.First();
				_actionValues[kvp.Key][kvp2.Key] = ActivationFunctions.Sigmoid(kvp2.Value);
			}
		}
	}

	private Dictionary<ActionType, float> SoftMax(Dictionary<ActionType, float> values) {
		var exp = values.ToDictionary(x => x.Key, x => Math.Exp(x.Value));
		var sum = exp.Values.Sum();
		var softmax = exp.ToDictionary(x => x.Key, x => (float)(x.Value / sum));
		return softmax;
	}

	private void FeedForward(World world) {
		foreach (var neuron in Brain.SortedNeurons) {
			var value = neuron switch {
				InputNeuron inputNeuron => GetSensoryInput(inputNeuron, world),
				ActionNeuron actionNeuron => CalculateNeuronValueFromDependencies(actionNeuron),
				_ => InternalActivationFunction(CalculateNeuronValueFromDependencies(neuron))
			};

			// add or update dict
			_neuronValues[neuron] = value;

			if (neuron.NeuronType == NeuronType.Action) {
				// fill in in _actionValues
				var actionNeuron = (ActionNeuron)neuron;
				var cat = actionNeuron.ActionType.GetActionCategory();
				_actionValues[cat][actionNeuron.ActionType] = value;
			}
		}
	}

	public float CalculateNeuronValueFromDependencies(Neuron neuron) {
		if (neuron.NeuronType == NeuronType.Input) {
			throw new InvalidOperationException("Cannot calculate value of input neuron");
		}

		if (neuron.NeuronType == NeuronType.Memory) {
			return GetNeuronValue(neuron);
		}

		var input = neuron.Bias;
		var incomingEdges = Brain.Dependencies[neuron];

		foreach (var neuronConnection in incomingEdges) {
			input += GetNeuronValue(neuronConnection.Source) * neuronConnection.Weight;
		}

		return input;
	}

	private float GetNeuronValue(Neuron neuron) {
		if (neuron.NeuronType == NeuronType.Memory) {
			neuron = neuron with { NeuronType = NeuronType.Internal };
		}

		if (!_neuronValues.TryGetValue(neuron, out var neuronValue)) {
			neuronValue = 0f;
		}

		return neuronValue;
	}

	private float GetSensoryInput(InputNeuron inputNeuron, World world) {
		return inputNeuron.InputType switch {
			InputType.LookFront => Look(world, Direction),
			InputType.LookLeft => Look(world, (Direction)((int)(Direction + 3) % 4)),
			InputType.LookRight => Look(world, (Direction)((int)(Direction + 1) % 4)),
			InputType.LookBack => Look(world, (Direction)((int)(Direction + 2) % 4)),
			InputType.SmellFood => 0,
			InputType.SmellPheromone => 0,
			InputType.Oscillator => Oscillate(),
			InputType.Hunger => 0,
			InputType.Tiredness => 0,
			InputType.PheromoneStrength => 0,
			InputType.PheromoneDecay => 0,
			InputType.IsEmittingPheromone => 0,
			InputType.Age => Age,
			InputType.Speed => 1,
			InputType.Collisions => Collisions,
			InputType.DistanceFromStart => ActivationFunctions.Sigmoid((float)StartPosition.CalculateDistanceBetweenPositions(Position)),
			InputType.StartX => (float)StartPosition.X / World.Width,
			InputType.StartY => (float)StartPosition.Y / World.Height,
			InputType.CurrentX => (float)Position.X / World.Width,
			InputType.CurrentY => (float)Position.Y / World.Height,
			InputType.Collided => Collided ? 1f : 0f,
			_ => 0
		};
	}

	private float Oscillate() => Oscillator.OscillateAnalog(OscillatorPhaseOffset + (float)Age / OscillatorAgeDivider, OscillatorFrequency);


	private float Look(World world, Direction direction) {
		// 1 = nothing infront
		float closestObj = 1;

		var closestWallInDirection = world.GetClosestWallInDirection(Position, direction);


		// check which is in the fov of the creature

		foreach (var worldCreature in world.Creatures) {
			if (this == worldCreature) {
				continue;
			}

			var distance = GetDistanceToCreature(worldCreature);

			if (distance > EyeSightStrength) {
				// we cannot see
				continue;
			}

			var angle = this.CalculateAngleBetweenCreatures(worldCreature, direction);

			if (angle >= ViewingAngle) {
				continue; // ?
			}

			var strength = 1 - (float)(distance / EyeSightStrength);
			closestObj = Math.Min(strength, closestObj);
		}

		return closestObj;
	}

	private double GetDistanceToCreature(Creature creature) {
		if (!_creatureDistances.TryGetValue(creature, out var value)) {
			value = this.CalculateDistanceBetweenCreatures(creature);
			//_creatureDistances.Add(creature, value);
		}

		return value;
	}

	public void Draw(SKCanvas canvas, Func<Vector, (int X, int Y)> calculatePixelPosition, Func<int, int> pixelSize) {
		var fillPaint = new SKPaint {
			Style = SKPaintStyle.Fill,
			Color = new SKColor(Color.R, Color.G, Color.B)
		};

		var pixelPosition = calculatePixelPosition(Position);

		canvas.DrawCircle(pixelPosition.X, pixelPosition.Y, pixelSize(Radius), fillPaint);
	}

	public void Deconstruct(out Genome genome, out Vector vector, out int radius, out Color color) {
		genome = Genome;
		vector = Position;
		radius = Radius;
		color = Color;
	}
}
