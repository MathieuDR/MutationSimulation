using Common.Helpers;
using SkiaSharp;

namespace Graphics.Helpers;

public static class SkiaHelpers {


	public static void SaveToStream(this SKSurface surface, Stream target, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100) {
		try {
			using var image = surface.Snapshot();

			if (image is null) {
				throw new InvalidOperationException("Could not get image from surface");
			}
		
			using var data = image.Encode(format, quality);
			data.SaveTo(target);
		} catch (Exception e) {
			Console.WriteLine(e);
			throw;
		}
		
	}
	
	public static void SaveToPath(this SKSurface surface, string path, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100) {
		// create directory if it doesn't exist
		FileHelper.EnsurePath(path);
		using var stream = File.OpenWrite(path);
		surface.SaveToStream(stream, format, quality);
	}
}
