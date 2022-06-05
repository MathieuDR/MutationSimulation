using Common.Helpers;
using Common.Models;
using Common.Models.Genetic.Components;
using Common.Models.Options;
using Common.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Factories;

public class WorldFactory {
	private readonly RenderOptions _renderOptions;
	private readonly CreatureFactory _creatureFactory;
	private readonly IServiceProvider _provider;
	private readonly Random _random;
	private Line[]? _walls;
	private Hotspot[]? _hotspots;
	private WorldOptions WorldOptions { get; init; }

	private Line[] Walls => _walls ??= CreateWalls();
	private Hotspot[] Hotspots => _hotspots ??= CreateHotspots();

	private Hotspot[]? CreateHotspots() {
		var count = _random.Next(3, 10);
		var hotspots = new Hotspot[count];
		for (int i = 0; i < count; i++) {
			hotspots[i] = new Hotspot() {
				Position =  _random.GetRandomPosition(WorldOptions.Width, WorldOptions.Height),
				Radius = _random.Next(10, 20)
			};
		}

		return hotspots;
	}


	public WorldFactory(IOptionsSnapshot<WorldOptions> worldOptions, IOptionsSnapshot<RenderOptions> renderOptions, CreatureFactory creatureFactory, IServiceProvider provider, IRandomProvider randomProvider) {
		_renderOptions = renderOptions.Value;
		_creatureFactory = creatureFactory;
		_provider = provider;
		_random = randomProvider.GetRandom();
		WorldOptions = worldOptions.Value;
	}

	public World Create(Genome[] genomes) {
		var creatures = GetCreatures(genomes);
		return new World(WorldOptions.Width, WorldOptions.Height, creatures) {
			Walls = Walls,
			Hotspots = Hotspots
		};
	}

	private Creature[] GetCreatures(Genome[] genomes) {
		// Ensure we have genomes for the first gen.
		if (genomes.Length == 0) {
			// Service locator anti pattern
			var factory = _provider.GetRequiredService<GenomeFactory>();
			genomes = factory.Create(WorldOptions.CreaturesAmount).ToArray();
		}

		var creatures = _creatureFactory.Create(genomes, Walls).ToArray();
		return creatures;
	}

	private Line[] CreateWalls() {
		var worldWalls = new List<Line>();
		
		// wall offset so whole wall is visible
		// on the edge of the wall
		var wallOffset = _renderOptions.WallWidth / 2;

		// top horizontal wall
		worldWalls.Add(new Line(
			new Vector(0, wallOffset),
			new Vector(WorldOptions.Width, wallOffset)
		));

		// bottom horizontal wall
		worldWalls.Add(new Line(
			new Vector(0, WorldOptions.Height - wallOffset),
			new Vector(WorldOptions.Width, WorldOptions.Height - wallOffset)
		));

		// left vertical wall
		worldWalls.Add(new Line(
			new Vector(wallOffset, 0),
			new Vector(wallOffset, WorldOptions.Height)
		));

		// right vertical wall
		worldWalls.Add(new Line(
			new Vector(WorldOptions.Width - wallOffset, 0),
			new Vector(WorldOptions.Width - wallOffset, WorldOptions.Height)
		));

		if (WorldOptions.ExtraWalls) {
			var xPoints = GetWallPoints(WorldOptions.Width);
			var yPoints = GetWallPoints(WorldOptions.Height);

			worldWalls.Add(new Line(
				new Vector(xPoints.Eight, yPoints.Half),
				new Vector(xPoints.SevenEights, yPoints.Half)));

			worldWalls.Add(new Line(
				new Vector(xPoints.Half, yPoints.Eight),
				new Vector(xPoints.Half, yPoints.SevenEights)));

			worldWalls.Add(new Line(
				new Vector(xPoints.Eight, yPoints.Half - yPoints.Eight),
				new Vector(xPoints.Half - xPoints.Eight, yPoints.Eight)));

			worldWalls.Add(new Line(
				new Vector(xPoints.Half + xPoints.Eight, yPoints.SevenEights),
				new Vector(xPoints.SevenEights, yPoints.Half + yPoints.Eight)));

			worldWalls.Add(new Line(
				new Vector(xPoints.Eight, yPoints.Half + yPoints.Eight),
				new Vector(xPoints.Half - xPoints.Eight, yPoints.SevenEights)));

			worldWalls.Add(new Line(
				new Vector(xPoints.Half + xPoints.Eight, yPoints.Eight),
				new Vector(xPoints.SevenEights, yPoints.Half - yPoints.Eight)));
		}

		return worldWalls.ToArray();
	}
	
	private (int Half, int Quarter, int Eight, int ThreeQuarter, int SevenEights) GetWallPoints(int size) {
		var half = size / 2;
		var quarter = size / 4;
		var eight = quarter / 2;
		var threeQuarter = quarter * 3;
		var sevenEight = eight * 7;

		return (half, quarter, eight, threeQuarter, sevenEight);
	}
}