﻿using System.Diagnostics;
using Common.Helpers;
using Common.Models;
using Common.Simulator;
using Graphics;
using Graphics.Helpers;

// var mark = BenchmarkRunner.Run<BenchMarker>();

int worldSize = 500;
var worldWalls = new List<Line>();
const int wallOffset = 2;
worldWalls.Add(new Line(new Vector(wallOffset, 0), new Vector(wallOffset, worldSize)));
worldWalls.Add(new Line(new Vector(0, wallOffset), new Vector(worldSize, wallOffset)));
worldWalls.Add(new Line(new Vector(worldSize - wallOffset, 0), new Vector(worldSize - wallOffset, worldSize)));
worldWalls.Add(new Line(new Vector(0, worldSize - wallOffset), new Vector(worldSize, worldSize - wallOffset)));
worldWalls.Add(new Line(new Vector(worldSize/4, worldSize/2), new Vector(worldSize/ 4 * 3, worldSize/2)));
worldWalls.Add(new Line(new Vector(worldSize/2, worldSize/4), new Vector(worldSize/2, worldSize/ 4 * 3)));
var walls = worldWalls.ToArray();

var watch = new Stopwatch();
RandomProvider.SetSeed(78123924);
var random = RandomProvider.GetRandom();


watch.Start();
var blobs = random.GetRandomCreatures(50, worldSize, worldSize, walls);
watch.Stop();
Console.WriteLine("Created {0} creatures in {1}ms", blobs.Length, watch.ElapsedMilliseconds);



var world = new World(worldSize, worldSize, blobs, walls);
var renderMachine = new WorldRenderMachine("output", "world");

foreach (var blob in blobs) {
	blob.Brain.CreateDotFile();
}

var images = new List<string>();
watch.Restart();
var rounds = 2;

for (var i = 0; i < rounds; i++) {
	var path = renderMachine.RenderWorld(world);
	if(!string.IsNullOrEmpty(path)) {
		images.Add(path);
	}

	if (i < rounds - 1) {
		world.NextTick();
	}
} 

watch.Stop();
Console.WriteLine("Rendered {0} images in {1}ms", images.Count, watch.ElapsedMilliseconds);

watch.Restart();
//Giffer.CreateGif(images, "./output/world.gif", 1);

watch.Stop();
Console.WriteLine("Created gif in {0}ms", watch.ElapsedMilliseconds);

Task.Delay(200);

Process.Start(new ProcessStartInfo() {
	FileName = "./output",
	UseShellExecute = true,
	Verb = "open"
});