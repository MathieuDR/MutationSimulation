namespace Common.Models; 

public record World(int Width, int Height, PhysicBlob[] Blobs, ulong Tick = 0);
