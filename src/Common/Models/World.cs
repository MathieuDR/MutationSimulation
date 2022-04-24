using Common.Interfaces;

namespace Common.Models; 

public record World(int Width, int Height, ICreature[] Blobs, ulong Tick = 0);
