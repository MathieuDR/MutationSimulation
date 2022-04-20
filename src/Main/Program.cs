// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using Graphics.Helpers;
using Graphics.Models;

var world = new World(256, 256, 5);
world.RenderSurface().SaveToPath("./output/world.png");


Process.Start(new ProcessStartInfo() {
	FileName = "./output",
	UseShellExecute = true,
	Verb = "open"
});