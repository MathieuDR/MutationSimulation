using SkiaSharp;

namespace Graphics; 


public interface IRenderEngine {
    Task Initialize();
    Task SaveFrame(string path);
    event Action<SKCanvas>? Render;
}