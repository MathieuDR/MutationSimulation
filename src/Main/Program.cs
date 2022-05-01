// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Common.Interfaces;
using Common.Models;
using Common.Simulator;
using Graphics;
using Graphics.Helpers;

// var mark = BenchmarkRunner.Run<BenchMarker>();

var seed = 8923478;
var watch = new Stopwatch();
var random = new Random(seed);

var hex =
	"AwRgHMqd0CySaAmGkIGYBiVLtXSFBfSeYI4PVeMq4M/W6REshlx06AVhPq4GdgvBBCEjyYnt1h4J6KTDLy+8IA==";
var genome = Genome.FromHex(hex);

watch.Start();
var brain = new Brain(genome);
watch.Stop();
Console.WriteLine("Brain took {0}ms", watch.ElapsedMilliseconds);

watch.Restart();
var dot = brain.CreateDotFile();
watch.Stop();
Console.WriteLine("Drawing brain took {0}ms", watch.ElapsedMilliseconds);
Console.WriteLine("Dot file: {0}", dot);

// int worldWidth = 150, worldHeight = 150;
//
// var blobs = Enumerable.Range(1,7).Select(_ => new BouncingCreature(random, 4, 20, worldWidth, worldHeight) as ICreature).ToArray();
//
// var world = new World(worldWidth, worldHeight, blobs);
// var renderMachine = new WorldRenderMachine("output", "world", randomSeed: seed);
//
// var images = new List<string>();
// watch.Start();
// for (var i = 0; i < 500; i++) {
// 	var path =renderMachine.RenderWorld(world);
// 	if(!string.IsNullOrEmpty(path)) {
// 		images.Add(path);
// 	}
// 	
// 	world = SimulationMachine.Tick(world);
// } 
//
// watch.Stop();
// Console.WriteLine("Rendered {0} images in {1}ms", images.Count, watch.ElapsedMilliseconds);
//
// watch.Restart();
// Giffer.CreateGif(images, "./output/world.gif", 1);
//
// watch.Stop();
// Console.WriteLine("Created gif in {0}ms", watch.ElapsedMilliseconds);

Task.Delay(200);

Process.Start(new ProcessStartInfo() {
	FileName = "./output",
	UseShellExecute = true,
	Verb = "open"
});