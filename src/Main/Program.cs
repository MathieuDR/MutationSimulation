// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using System.Reflection.Metadata;
using Common.Helpers;
using Common.Models;
using Common.Models.Genetic;
using Common.Models.Genetic.Components;
using Common.Models.Genetic.Components.Neurons;
using Common.Simulator;
using Graphics;
using Graphics.Helpers;

// var mark = BenchmarkRunner.Run<BenchMarker>();

var watch = new Stopwatch();
RandomProvider.SetSeed(8923478);
var random = RandomProvider.GetRandom();

var connections = new[] {
	new NeuronConnection(new InputNeuron(13), new ActionNeuron(11), NeuronConnection.WeightToFloat(3f)),
	new NeuronConnection(new InputNeuron(13), new ActionNeuron(3), NeuronConnection.WeightToFloat(-3f)),
};

var genome = new Genome(connections);
//
// watch.Start();
// var brain = new Brain(genome);
// watch.Stop();
// Console.WriteLine("Brain took {0}ms", watch.ElapsedMilliseconds);
//
// watch.Restart();
// var dot = brain.CreateDotFile();
// watch.Stop();
// Console.WriteLine("Drawing brain took {0}ms", watch.ElapsedMilliseconds);
// Console.WriteLine("Dot file: {0}", dot);

int worldSize = 200;
var blobRadius = 10;
// var blobAmount = worldSize * worldSize / (blobRadius / 2);
// var blobsPerRow = worldSize / blobRadius;
//
// var middle = new Position(worldSize / 2, worldSize / 2);
// var viewingAngle = 135;
//
// var blobs = new Creature[blobAmount+1];
// var defColor = new Color(255, 255, 255);
//
// for (int i = 0; i < blobAmount; i++) {
// 	var x = (blobRadius * 2) * (i % blobsPerRow) + blobRadius;
// 	var y = (blobRadius * 2) * (i / blobsPerRow) + blobRadius;
// 	var pos = new Position(x, y);
//
// 	var westAngle = middle.CalculateAngleBetweenPositions(pos);
// 	var northAngle = PositionHelper.ChangeDefaultAngleToDirection(westAngle, Direction.North);
// 	var southAngle = PositionHelper.ChangeDefaultAngleToDirection(westAngle, Direction.South);
// 	var eastAngle = PositionHelper.ChangeDefaultAngleToDirection(westAngle, Direction.East);
// 	
// 	var isInNorthAngle = PositionHelper.IsInViewingAngle(northAngle, viewingAngle);
// 	var isInSouthAngle = PositionHelper.IsInViewingAngle(southAngle, viewingAngle);
// 	var isInWestAngle = PositionHelper.IsInViewingAngle(westAngle, viewingAngle);
// 	var isInEastAngle = PositionHelper.IsInViewingAngle(eastAngle, viewingAngle);
//
// 	Color color = defColor;
//
// 	if (isInNorthAngle && isInEastAngle) {
// 		color = new Color(255, 0, 255); // pink
// 	}else if (isInNorthAngle && isInWestAngle) {
// 		color = new Color(255, 255, 0); // yellow
// 	}else if (isInNorthAngle) {
// 		color = new Color(255, 0, 0); // red
// 	}else if (isInSouthAngle && isInEastAngle) {
// 		color = new Color(0, 0, 128);  // dark blue
// 	}else if (isInSouthAngle && isInWestAngle) {
// 		color = new Color(0, 128, 0); // dark green
// 	}else if (isInSouthAngle) {
// 		color = new Color(0, 0, 0); // black
// 	}else if (isInWestAngle) {
// 		color = new Color(0, 255, 0); // green
// 	}else if (isInEastAngle) {
// 		color = new Color(0, 0, 255); // blue
// 	}
//
// 	blobs[i] = new Creature(genome, pos, blobRadius, color);
// }
//
// blobs[^1] = new Creature(genome, middle, blobRadius, new Color(84, 65, 181));

var blobs = new Creature[10];
blobs = blobs.Select(x=> new Creature(genome, new Position(random, worldSize, worldSize, blobRadius), blobRadius, new Color(random))).ToArray();

blobs.First().Brain.CreateDotFile();


var world = new World(worldSize, worldSize, blobs);
var renderMachine = new WorldRenderMachine("output", "world");

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
//Giffer.CreateGif(images, "./output/world.gif", 1);

watch.Stop();
Console.WriteLine("Created gif in {0}ms", watch.ElapsedMilliseconds);

Task.Delay(200);

Process.Start(new ProcessStartInfo() {
	FileName = "./output",
	UseShellExecute = true,
	Verb = "open"
});