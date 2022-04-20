namespace Common.Models; 

public record World(int Width, int Height, Blob[] Blobs, ulong Tick = 0);
