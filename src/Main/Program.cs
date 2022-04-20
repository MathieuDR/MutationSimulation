// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Common.Models;
using Common.Simulator;
using Graphics;

var seed = 45124;
var random = new Random(seed);
var blobs = new Blob[2];

int worldWidth = 256, worldHeight = 256, maxVelocity = 5, maxDiameter = 25;

// generate random blobs
for (var i = 0; i < blobs.Length; i++) {
	var diameter = random.Next(10, maxDiameter);
	blobs[i] = new Blob(new Position(random.Next(diameter, worldWidth-diameter), random.Next(diameter, worldHeight-diameter)), 
		new Velocity(random.Next(1, maxVelocity), random.Next(1, maxVelocity)), diameter, new Color(random));
}

var world = new World(worldWidth, worldHeight, blobs);


var renderMachine = new WorldRenderMachine("output", "world", randomSeed: seed);

for (var i = 0; i < 255; i++) {
	renderMachine.RenderWorld(world);
	world = SimulationMachine.Tick(world);
}


Process.Start(new ProcessStartInfo() {
	FileName = "./output",
	UseShellExecute = true,
	Verb = "open"
});