namespace Common.Models; 

public record World(int Width, int Height, Creature[] Creatures, ulong Tick = 0);
