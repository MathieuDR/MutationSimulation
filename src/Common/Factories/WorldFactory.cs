using Common.Models;
using Common.Models.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Common.Factories;

public static class WorldFactory {
	public static World CreateWorld(IServiceProvider serviceProvider) {
		var worldOptions = serviceProvider.GetRequiredService<IOptionsSnapshot<WorldOptions>>().Value;
		
		var walls = worldOptions.CreateWalls().ToArray();
		var creatures = CreatureFactory.CreateCreatures(serviceProvider, worldOptions.CreaturesAmount, walls);
		var world = worldOptions.FromOptions(creatures, walls);

		return world;
	}

	private static World FromOptions(this WorldOptions options, IEnumerable<Creature> creatures, IEnumerable<Line>? walls = null) {
		walls ??= options.CreateWalls();
		var result = new World(options.Width, options.Height, creatures.ToArray(), walls.ToArray());
		return result;
	}

	private static IEnumerable<Line> CreateWalls(this WorldOptions options) {
		var worldWalls = new List<Line>();
		
		// wall offset so whole wall is visible
		// on the edge of the wall
		var wallOffset = options.WallWidth / 2;

		// top horizontal wall
		worldWalls.Add(new Line(
			new Vector(0, wallOffset),
			new Vector(options.Width, wallOffset)
		));

		// bottom horizontal wall
		worldWalls.Add(new Line(
			new Vector(0, options.Height - wallOffset),
			new Vector(options.Width, options.Height - wallOffset)
		));

		// left vertical wall
		worldWalls.Add(new Line(
			new Vector(wallOffset, 0),
			new Vector(wallOffset, options.Height)
		));

		// right vertical wall
		worldWalls.Add(new Line(
			new Vector(options.Width - wallOffset, 0),
			new Vector(options.Width - wallOffset, options.Height)
		));

		if (options.ExtraWalls) {
			var xPoints = GetWallPoints(options.Width);
			var yPoints = GetWallPoints(options.Height);

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

		return worldWalls;
	}

	private static (int Half, int Quarter, int Eight, int ThreeQuarter, int SevenEights) GetWallPoints(int size) {
		var half = size / 2;
		var quarter = size / 4;
		var eight = quarter / 2;
		var threeQuarter = quarter * 3;
		var sevenEight = eight * 7;

		return (half, quarter, eight, threeQuarter, sevenEight);
	}
}
