using System.Diagnostics;
using Common.Helpers;
using Common.Models;
using Common.Simulator;
using Graphics;
using Graphics.Helpers;

// var mark = BenchmarkRunner.Run<BenchMarker>();

var watch = new Stopwatch();
RandomProvider.SetSeed(781239324);
var random = RandomProvider.GetRandom();

int worldSize = 500;
var blobs = random.GetRandomCreatures(50, worldSize, worldSize);


var world = new World(worldSize, worldSize, blobs);
var renderMachine = new WorldRenderMachine("output", "world");

foreach (var blob in blobs) {
	blob.Brain.CreateDotFile();
}

var images = new List<string>();
watch.Start();
var rounds = 500;

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
Giffer.CreateGif(images, "./output/world.gif", 1);

watch.Stop();
Console.WriteLine("Created gif in {0}ms", watch.ElapsedMilliseconds);

Task.Delay(200);

Process.Start(new ProcessStartInfo() {
	FileName = "./output/brains",
	UseShellExecute = true,
	Verb = "open"
});