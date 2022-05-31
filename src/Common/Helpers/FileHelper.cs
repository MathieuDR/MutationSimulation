namespace Common.Helpers; 

public static class FileHelper {
	public static void EnsurePath(string filePath) {
		var path = Path.GetDirectoryName(filePath);

		if (string.IsNullOrEmpty(path)) {
			throw new InvalidCastException("Invalid file path");
		}
		
		if (!Directory.Exists(path)) {
			Directory.CreateDirectory(path);
		}
	}
}
