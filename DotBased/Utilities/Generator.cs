namespace DotBased.Utilities;

public static class Generator
{
    private static readonly Random Random = new Random();
    
    public static string GenerateRandomHexColor() => $"#{Random.Next(0x1000000):X6}";
}