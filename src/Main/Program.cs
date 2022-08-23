using CommandLine;
using Common.Factories;
using Common.FitnessTests;
using Common.FitnessTests.Parts;
using Common.Models;
using Common.Models.Genetic;
using Common.Models.Genetic.Components;
using Common.Models.Options;
using Common.Services;
using Graphics;
using Graphics.RenderEngines;
using Main.Models;
using Main.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;

namespace Main; 

public class Program {
	public static void Main(string[] args) {
		CreateLogger();
		
		try {
			Parser.Default.ParseArguments<CmdArgs>(args)
				.WithParsed(opt => {
					var host = CreateHostBuilder(opt, args).Build();
					if (opt.RenderGui) {
						
					}
					host.Run();
				});
		} catch (Exception e) {
			Log.Logger.Fatal(e, "Fatal exception");
		}
	}

	static IHostBuilder CreateHostBuilder(CmdArgs options, string[] args) {
		return Host
			.CreateDefaultBuilder(args)
			.ConfigureAppConfiguration(builder => { BuildConfiguration(builder, options); })
			.ConfigureServices((hostContext, services) => {
				var customOptions = hostContext.Configuration.GetSection("Mutation");

				services.AddOptions<BrainOptions>().Bind(customOptions.GetSection(BrainOptions.SectionName)).Validate(ValidateOptions);
				services.AddOptions<CreatureOptions>().Bind(customOptions.GetSection(CreatureOptions.SectionName)).Validate(ValidateOptions);
				services.AddOptions<RandomOptions>().Bind(customOptions.GetSection(RandomOptions.SectionName)).Validate(ValidateOptions);
				services.AddOptions<RenderOptions>().Bind(customOptions.GetSection(RenderOptions.SectionName)).Validate(ValidateOptions);
				services.AddOptions<SimulatorOptions>().Bind(customOptions.GetSection(SimulatorOptions.SectionName)).Validate(ValidateOptions);
				services.AddOptions<WorldOptions>().Bind(customOptions.GetSection(WorldOptions.SectionName)).Validate(ValidateOptions);
				services.AddOptions<FitnessOptions>().Bind(customOptions.GetSection(FitnessOptions.SectionName)).Validate(ValidateOptions);

				services.AddSingleton<SilkWindow>();


				var opts = new FitnessOptions();
				customOptions.Bind(FitnessOptions.SectionName, opts);

				services.AddSingleton<IFitnessPart, FitnessStart>();
				foreach (var part in opts.Chain) {
					Func<IFitnessPart, IFitnessPart> decorator = part.Part switch {
						FitnessPart.DistanceFromStart => inner => new DistanceFromStartFitnessDecorator(inner, part.Multiplier),
						FitnessPart.Distance => inner => new DistanceTravelledFitnessDecorator(inner, part.Multiplier),
						FitnessPart.UniqueDistance => inner => new UniqueDistanceTravelledFitnessDecorator(inner, part.Multiplier),
						FitnessPart.NoCollisions => inner => new NoCollisionsFitnessDecorator(inner, part.Multiplier),
						FitnessPart.HotspotCollector => inner => new HotspotCollectorDecorator(inner, part.Multiplier),
						FitnessPart.UniqueDistanceCombo => inner => new LocationComboDecorator(inner, part.Multiplier),
						_ => throw new ArgumentOutOfRangeException()
					};

					services.Decorate(decorator);
				}

				services.AddScoped<IRandomProvider, RandomProvider>();
				services.AddScoped<ContextProvider>();
				services.AddScoped<Simulator>();
				services.AddScoped<WorldFactory>();
				services.AddScoped<GenerationSolver>();
				services.AddScoped<Mutator>();
				services.AddScoped<CreatureFactory>();
				services.AddScoped<GenomeFactory>();
				services.AddScoped<RenderMachine>();
				services.AddSingleton<IFitnessScorer, FitnessScorer>();
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
			.Destructure.ByTransforming<OldGenome>(
				g => g.HexSequence ?? g.ToHex())
			.Destructure.ByTransforming<Brain>(
				b => new {
					Edges = b.BrainGraph.Edges.Count(),
					Vertices = b.BrainGraph.Vertices.Count()
				})
			.Destructure.ByTransforming<Creature>(
				c => {

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

}

// Process.Start(new ProcessStartInfo {
// 	FileName = "./output",
// 	UseShellExecute = true,
// 	Verb = "open"
// });
