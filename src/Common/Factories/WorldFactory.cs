using Common.Models;
using Common.Models.Options;
using Microsoft.Extensions.Options;

namespace Common.Factories;

public class WorldFactory {
	private readonly RenderOptions _renderOptions;
	private readonly CreatureFactory _creatureFactory;
	private Line[]? _walls;
	private Creature[]? _creatures;
	private WorldOptions WorldOptions { get; init; }

	private Line[] Walls => _walls ??= CreateWalls();

	private Creature[] Creatures => _creatures ??= CreateCreatures();

	public WorldFactory(IOptionsSnapshot<WorldOptions> worldOptions, IOptionsSnapshot<RenderOptions> renderOptions, CreatureFactory creatureFactory) {
		_renderOptions = renderOptions.Value;
		_creatureFactory = creatureFactory;
		WorldOptions = worldOptions.Value;
	}

	public World Create() => new World(WorldOptions.Width, WorldOptions.Height, Creatures, Walls);

	private Creature[] CreateCreatures() {
		return _creatureFactory.Create(WorldOptions.CreaturesAmount, Walls).ToArray();
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