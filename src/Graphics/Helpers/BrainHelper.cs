using Common.Models;
using Common.Models.Bio;
using QuikGraph.Graphviz;
using QuikGraph.Graphviz.Dot;

namespace Graphics.Helpers; 

public static class BrainHelper {
	public static string CreateDotFile(this Brain brain, string outputDir = "./output/brains/", string? fileName = null) {
		var dot = CreateDotContent(brain);
		
		// create output directory if it doesn't exist
		if (!Directory.Exists(outputDir)) {
			Directory.CreateDirectory(outputDir);
		}
		
		var filePath = Path.Combine(outputDir, ToValidFileName(fileName ?? $"{brain.Genome.HexSequence}.dot"));

		File.WriteAllText(filePath, dot);
		return filePath;
	}

	private static string ToValidFileName(string filename) {
		var invalidChars = Path.GetInvalidFileNameChars();
		return new string(filename.Select(c => invalidChars.Contains(c) ? '_' : c).ToArray());
	}

	private static string CreateDotContent(Brain brain) {
		return brain.BrainGraph.ToGraphviz(alg => {
			alg.ImageType = GraphvizImageType.Png;
			alg.FormatVertex += (sender, args) => {
				var vertex = args.Vertex;
				switch (vertex.NeuronType) {
					case NeuronType.Input:
						args.VertexFormat.Group = "input";
						args.VertexFormat.FillColor = GraphvizColor.Green;
						break;
					case NeuronType.Action:
						args.VertexFormat.Group = "output";
						args.VertexFormat.FillColor = GraphvizColor.Orange;
						break;
					case NeuronType.Internal:
						args.VertexFormat.Group = "internal";
						args.VertexFormat.FillColor = GraphvizColor.Yellow;
						break;
					case NeuronType.Memory:
						args.VertexFormat.Group = "memory";
						args.VertexFormat.FillColor = GraphvizColor.Purple;
						args.VertexFormat.StrokeColor = GraphvizColor.White;
						args.VertexFormat.Style = GraphvizVertexStyle.Dotted;
						break;
					default:
						throw new ArgumentOutOfRangeException();
				}

				args.VertexFormat.Style = GraphvizVertexStyle.Filled;
				args.VertexFormat.Label = vertex.Id.ToString();
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
