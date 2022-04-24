using Common.Interfaces;

namespace Common.Models; 

public record World(int Width, int Height, ICreature[] Creatures, ulong Tick = 0);
