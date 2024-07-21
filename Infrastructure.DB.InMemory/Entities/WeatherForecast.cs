namespace Infrastructure.DB.InMemory.Entities;

public enum SummaryEnum
{
    Freezing,
    Bracing,
    Chilly,
    Cool,
    Mild,
    Warm,
    Balmy,
    Hot,
    Sweltering,
    Scorching
}

public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
