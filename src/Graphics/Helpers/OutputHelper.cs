namespace Graphics.Helpers; 

internal class OutputHelper {
	public static void EnsureDirectory(string path) {
		var dir = Path.GetDirectoryName(path);
		if (dir is not null && !Directory.Exists(dir)) {
			Directory.CreateDirectory(dir);
		}
	}
}
