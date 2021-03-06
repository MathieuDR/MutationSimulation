using Common.Helpers;
using Common.Models.Genetic;
using Common.Models.Genetic.Components.Neurons;
using MathieuDR.Common.Extensions;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;

namespace Graphics.Helpers; 

public static class BrainHelper {
	public static string CreateDotFile(this Brain brain, string outputDir = "./output/brains/", string? fileName = null) {
		var dot = CreateDotContent(brain);
		
		FileHelper.EnsurePath(outputDir);
		
		var filePath = Path.Combine(outputDir, ToValidFileName(fileName ?? $"{brain.Genome.HexSequence}.dot"));

		File.WriteAllText(filePath, dot);
		return filePath;
	}

	private static string ToValidFileName(string filename) {
		var invalidChars = Path.GetInvalidFileNameChars();
		var proposed = new string(filename.Select(c => invalidChars.Contains(c) ? '_' : c).ToArray());
		
		return proposed.Length > 200 ? proposed.Substring(0, 200) + ".dot" : proposed;
	}

	private static string CreateDotContent(Brain brain) {
		return brain.BrainGraph.ToGraphviz(alg => {
			alg.ImageType = GraphvizImageType.Png;
			alg.FormatVertex += (sender, args) => {
				var vertex = args.Vertex;
				switch (vertex.NeuronType) {
					case NeuronType.Input:
						args.VertexFormat.Group = "input";
						args.VertexFormat.Label = vertex.Cast<InputNeuron>().InputType.ToString();
						args.VertexFormat.FillColor = GraphvizColor.Green;
						break;
					case NeuronType.Action:
						args.VertexFormat.Group = "output";
						args.VertexFormat.Label = vertex.Cast<ActionNeuron>().ActionType.ToString();
						args.VertexFormat.FillColor = GraphvizColor.Orange;
						break;
					case NeuronType.Internal:
						args.VertexFormat.Group = "internal";
						args.VertexFormat.Label = vertex.Id.ToString();
						args.VertexFormat.FillColor = GraphvizColor.Yellow;
						break;
					case NeuronType.Memory:
						args.VertexFormat.Group = "memory";
						args.VertexFormat.Label = vertex.Id.ToString();
						args.VertexFormat.FillColor = GraphvizColor.Purple;
						args.VertexFormat.StrokeColor = GraphvizColor.White;
						args.VertexFormat.Style = GraphvizVertexStyle.Dotted;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				args.VertexFormat.Style = GraphvizVertexStyle.Filled;
			};

			alg.FormatEdge += (sender, args) => {
				var edge = args.Edge;

				var isRed = edge.Weight < 0;
				args.EdgeFormat.Style = edge.Source.NeuronType == NeuronType.Memory ? GraphvizEdgeStyle.Dashed : GraphvizEdgeStyle.Solid;
				args.EdgeFormat.FontColor = isRed ? GraphvizColor.Red : GraphvizColor.Black;

				args.EdgeFormat.Label = new GraphvizEdgeLabel() {
					Value = edge.Weight.ToString("E3"),
					FontColor = isRed ? GraphvizColor.Red : GraphvizColor.Black
				};
			};
		});
	}
}
