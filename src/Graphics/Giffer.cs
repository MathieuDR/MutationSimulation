using Graphics.Helpers;
using ImageMagick;

namespace Graphics; 

public static class Giffer {

	public static string CreateGif(IEnumerable<string> paths, string outputPath, int delay = 7) {
		FileHelper.EnsurePath(outputPath);
		var pathsList = paths.ToArray();
		
		var settings = new QuantizeSettings();
		settings.Colors = 128;

		using var collection = new MagickImageCollection();

		for (var i = 0; i < pathsList.Length; i++) {
			var path = pathsList[i];
			collection.Add(path);
			collection[i].AnimationDelay = delay;
		}


		// Optionally reduce colors
		collection.Quantize(settings);

		// Save gif
		collection.Write(outputPath);
		return outputPath;
	}
}
