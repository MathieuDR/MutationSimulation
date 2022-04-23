// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using BenchmarkDotNet.Running;
using Common.Models;
using Common.Simulator;
using Graphics;
using LanguageExt;
using Main;

var mark = BenchmarkRunner.Run<BenchMarker>();

// var seed = 8923478;
// var watch = new Stopwatch();
// var random = new Random(seed);
//
// int worldWidth = 200, worldHeight = 100;
//
// var blobs = IntegerRange.FromMinMax(1, 7, 1).Select(_ => new PhysicBlob(random, 4, 20, worldWidth, worldHeight)).ToArray();
//
// var world = new World(worldWidth, worldHeight, blobs);
// var renderMachine = new WorldRenderMachine("output", "world", randomSeed: seed);
//
// var images = new List<string>();
// watch.Start();
// for (var i = 0; i < 500; i++) {
// 	var options =renderMachine.RenderWorld(world);
// 	if(options.IsSome) {
// 		images.Add(options.Select(x => x).First());
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
//
// Task.Delay(200);
//
// Process.Start(new ProcessStartInfo() {
// 	FileName = "./output",
// 	UseShellExecute = true,
// 	Verb = "open"
// });