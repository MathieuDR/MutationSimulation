// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Common.Models;
using Common.Simulator;
using Graphics;

var seed = 50023;
var random = new Random(seed);
var blobs = new Blob[10];

int worldWidth = 256, worldHeight = 256, maxVelocity = 5, maxDiameter = 20;

// generate random blobs
for (var i = 0; i < blobs.Length; i++) {
	var diameter = random.Next(5, maxDiameter);
	blobs[i] = new Blob(new Position(random.Next(diameter, worldWidth-diameter), random.Next(diameter, worldHeight-diameter)), 
		new Velocity(random.Next(1, maxVelocity), random.Next(1, maxVelocity)), diameter);
}

var world = new World(worldWidth, worldHeight, blobs);


var renderMachine = new WorldRenderMachine("output", "world", randomSeed: seed);

for (var i = 0; i < 100; i++) {
	renderMachine.RenderWorld(world);
	world = SimulationMachine.Tick(world);
}

var result = renderMachine.RenderWorld(world);

Process.Start(new ProcessStartInfo() {
	FileName = "./output",
	UseShellExecute = true,
	Verb = "open"
});