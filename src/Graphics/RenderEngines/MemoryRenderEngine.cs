using Common.Models.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SkiaSharp;

namespace Graphics.RenderEngines;

public class MemoryRenderEngine : IRenderEngine, IDisposable {
    private readonly ILogger<MemoryRenderEngine> _logger;
    private readonly RenderOptions _renderOptions;
    private readonly WorldOptions _worldOptions;
    
    public MemoryRenderEngine(ILogger<MemoryRenderEngine> logger, IOptionsSnapshot<RenderOptions> renderOptions, IOptionsSnapshot<WorldOptions> worldOptions) {
        _logger = logger;
        _renderOptions = renderOptions.Value;
        _worldOptions = worldOptions.Value;
    }
    
    public Task Initialize() => throw new NotImplementedException();

    public Task SaveFrame(string path) => throw new NotImplementedException();

    public event Action<SKCanvas>? Render;
    public void Dispose() {
        throw new NotImplementedException();
    }
}
