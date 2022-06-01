using CommandLine;
using Common.Factories;
using Common.FitnessTests.Parts;
using Common.Helpers;
using Common.Models;
using Common.Models.Genetic;
using Common.Models.Genetic.Components;
using Common.Models.Options;
using Common.Services;
using Graphics;
using Main.Models;
using Main.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

CreateLogger();

try {
	Parser.Default.ParseArguments<CmdArgs>(args)
		.WithParsed(opt => {
			CreateHostBuilder(opt, args).Build().Run();
		});
} catch (Exception e) {
	Log.Logger.Fatal(e, "Fatal exception");
}

static IHostBuilder CreateHostBuilder(CmdArgs options, string[] args) {
	return Host
		.CreateDefaultBuilder(args)
		.ConfigureAppConfiguration(builder => { BuildConfiguration(builder, options); })
		.ConfigureServices((hostContext, services) => {
			var customOptions = hostContext.Configuration.GetSection("Mutation");
			
			services
				.AddOptions<BrainOptions>()
				.Bind(customOptions.GetSection(BrainOptions.SectionName))
				.Validate(options =>  {
					var isValid = options.Validate(out var results);
				
					if (!isValid) {
						Log.Error("Following errors occured: {@errors}", results);
					}

					return isValid;
				});

			
			services.AddOptions<CreatureOptions>().Bind(customOptions.GetSection(CreatureOptions.SectionName)).Validate(ValidateOptions);
			services.AddOptions<RandomOptions>().Bind(customOptions.GetSection(RandomOptions.SectionName)).Validate(ValidateOptions);
			services.AddOptions<RenderOptions>().Bind(customOptions.GetSection(RenderOptions.SectionName)).Validate(ValidateOptions);
			services.AddOptions<SimulatorOptions>().Bind(customOptions.GetSection(SimulatorOptions.SectionName)).Validate(ValidateOptions);
			services.AddOptions<WorldOptions>().Bind(customOptions.GetSection(WorldOptions.SectionName)).Validate(ValidateOptions);
			services.AddOptions<FitnessOptions>().Bind(customOptions.GetSection(FitnessOptions.SectionName)).Validate(ValidateOptions);


			var opts = new FitnessOptions();
			customOptions.Bind(FitnessOptions.SectionName, opts);

			services.AddSingleton<IFitnessPart, FitnessStart>();
			foreach (var part in opts.Chain) {
				Func<IFitnessPart, IFitnessPart> decorator = part.Part switch {
					FitnessPart.DistanceFromStart => inner => new DistanceFromStartFitnessDecorator(inner, part.Multiplier),
					FitnessPart.Distance => inner => new DistanceTravelledFitnessDecorator(inner, part.Multiplier),
					FitnessPart.UniqueDistance => inner => new UniqueDistanceTravelledFitnessDecorator(inner, part.Multiplier),
					FitnessPart.NoCollisions => inner => new NoCollisionsFitnessDecorator(inner, part.Multiplier),
					_ => throw new ArgumentOutOfRangeException()
				};

				services.Decorate(decorator);
			}
			
			services.AddScoped<IRandomProvider, RandomProvider>();
			services.AddScoped<ContextProvider>();
			services.AddScoped<Simulator>();
			services.AddScoped<WorldFactory>();
			services.AddScoped<CreatureFactory>();
			services.AddScoped<GenomeFactory>();
			services.AddScoped<RenderMachine>();
			services.AddSingleton<GifRenderer>();
			services.AddScoped<GenerationContext>(provider => {
				var ctxProvider = provider.GetRequiredService<ContextProvider>();
				return ctxProvider.Context ?? throw new InvalidOperationException("Context has not been initialized yet");
			});
			services.AddHostedService<SimulationHost>();
		})
		.UseSerilog();
}

static bool ValidateOptions(ConfigurationOptions options) {
	var isValid = options.Validate(out var results);
				
	if (!isValid) {
		Log.Error("Configuration errors: {@errors}", results);
	}

	return isValid;
}

static void CreateLogger() {
	Log.Logger = new LoggerConfiguration()
		.Destructure.ByTransforming<Genome>(
			g=> g.HexSequence ?? g.ToHex())
		.Destructure.ByTransforming<Brain>(
			b=> new {
				Edges = b.BrainGraph.Edges.Count(),
				Vertices = b.BrainGraph.Vertices.Count()
			})
		.Destructure.ByTransforming<Creature>(
			c=> {

				if (Log.IsEnabled(LogEventLevel.Verbose)) {
					return new {
						Genome = c.Genome,
						Brain = c.Brain,
						Id = c.Id,
						Score = 0,
						Age = c.Age
					};
				}
				
				return new {
					Id = c.Id,
					Score = 0,
				};
			})
		.Enrich.FromLogContext()
		.MinimumLevel.Debug()
		.WriteTo.File(new JsonFormatter(), "logs/output.log", rollingInterval: RollingInterval.Day)
		.WriteTo.Console(LogEventLevel.Information)
		.CreateLogger();
}

static void BuildConfiguration(IConfigurationBuilder builder, CmdArgs args) {
	var env = args.Environment ?? Environment.GetEnvironmentVariable("SIM-ENVIRONMENT");
	builder.Sources.Clear();
	builder.AddJsonFile("appsettings.json", false, true);
	builder.AddJsonFile($"appsettings.{env.ToLowerInvariant()}.json", true, true);
}

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
