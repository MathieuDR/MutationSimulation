using System.Runtime.CompilerServices;

namespace ByteEncoder.Unit;

public static class VerifySettings {
    [ModuleInitializer]
    public static void Initialize() =>
        VerifierSettings.DerivePathInfo((sourceFile, projectDirectory, type, method) => new PathInfo(
            Path.Combine(projectDirectory, ".snapshots"),
            type.Name,
            method.Name));
}
