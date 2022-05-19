using System.Diagnostics;
using CommandLine;
using Common.Helpers;
using Common.Models;
using Common.Simulator;
using Graphics;
using Graphics.Helpers;
using Main.Models;

CommandLine.Parser.Default.ParseArguments<Options>(args)
	.WithParsed((opt) => {
		Console.WriteLine("Got options");
	});

// var worldSize = 500;
// var worldWalls = new List<Line>();
// var offset = worldSize / 10;
// const int wallOffset = 2;
// var half = worldSize / 2;
// var quarter = worldSize / 4;
// var eight = quarter / 2;
// var threeQuarter = quarter * 3;
// var sevenEight = eight * 7;
//
// worldWalls.Add(new Line(new Vector(wallOffset, 0), new Vector(wallOffset, worldSize)));
// worldWalls.Add(new Line(new Vector(0, wallOffset), new Vector(worldSize, wallOffset)));
// worldWalls.Add(new Line(new Vector(worldSize - wallOffset, 0), new Vector(worldSize - wallOffset, worldSize)));
// worldWalls.Add(new Line(new Vector(0, worldSize - wallOffset), new Vector(worldSize, worldSize - wallOffset)));
// worldWalls.Add(new Line(new Vector(eight, half), new Vector(sevenEight, half)));
// worldWalls.Add(new Line(new Vector(half, eight), new Vector(half, sevenEight)));
//
// worldWalls.Add(new Line(new Vector(eight, half - eight), new Vector(half - eight, eight)));
// worldWalls.Add(new Line(new Vector(half + eight, sevenEight), new Vector(sevenEight, half + eight)));
// worldWalls.Add(new Line(new Vector(eight, half + eight), new Vector(half - eight, sevenEight)));
// worldWalls.Add(new Line(new Vector(half + eight, eight), new Vector(sevenEight, half - eight)));
//
//
// var walls = worldWalls.ToArray();
//
// var watch = new Stopwatch();
// RandomProvider.SetSeed("linatje");
// var random = RandomProvider.GetRandom();
//
//
// watch.Start();
// var blobs = random.GetRandomCreatures(100, worldSize, worldSize, walls);
// watch.Stop();
// Console.WriteLine("Created {0} creatures in {1}ms", blobs.Length, watch.ElapsedMilliseconds);
//
// var world = new World(worldSize, worldSize, blobs, walls);
// var renderMachine = new WorldRenderMachine("output", "world");
//
// foreach (var blob in blobs) {
// 	blob.Brain.CreateDotFile(fileName: $"{blob.Id}.dot");
// }
//
// var images = new List<string>();
// watch.Restart();
// var rounds = 1;
//
// for (var i = 0; i < rounds; i++) {
// 	var path = renderMachine.RenderWorld(world);
// 	if (!string.IsNullOrEmpty(path)) {
// 		images.Add(path);
// 	}
//
// 	if (i < rounds - 1) {
// 		world.NextTick();
// 	}
// }
//
// watch.Stop();
// Console.WriteLine("Rendered {0} images in {1}ms", images.Count, watch.ElapsedMilliseconds);
//
//
// var l = world.Creatures
// 	.Select(x => (UniquePositions: x.UniquePositions.Count, x.Collisions, x.Distance, x.Speed,
// 		DistanceFromStart: x.Position.CalculateDistanceBetweenPositions(x.StartPosition), x.Id))
// 	.Select(x => (x.UniquePositions, x.Collisions, x.DistanceFromStart, x.Distance, x.Id,
// 		Score: x.DistanceFromStart * 5 + x.Distance / 2 - x.Collisions * 7 + x.UniquePositions * x.Speed))
// 	.OrderByDescending(x => x.Score)
// 	.ToList();
//
// foreach (var creature in l.Take(l.Count / 2)) {
// 	Console.WriteLine(
// 		"Visited {0} unique positions with {1} collisions and a distance of {2:F1} and a total distance of {5:F1} giving a score of {4}. ({3})",
// 		creature.UniquePositions, creature.Collisions, creature.DistanceFromStart, creature.Id, creature.Score, creature.Distance);
// }
//
// Console.WriteLine(string.Join(", ", l.Take(5).Select(x => x.Id.ToString())));
//
// watch.Restart();
// Giffer.CreateGif(images, "./output/world.gif", 1);
//
// watch.Stop();
// Console.WriteLine("Created gif in {0}ms", watch.ElapsedMilliseconds);
//
// Task.Delay(200);
//
// Process.Start(new ProcessStartInfo {
// 	FileName = "./output",
// 	UseShellExecute = true,
// 	Verb = "open"
// });
