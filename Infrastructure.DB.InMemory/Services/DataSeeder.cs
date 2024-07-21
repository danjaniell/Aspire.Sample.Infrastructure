using Bogus;
using Infrastructure.DB.InMemory.Entities;

namespace Infrastructure.DB.InMemory.Services;

public class DataSeeder
{
    public Dictionary<int, WeatherForecast> db = [];
    private int currentId = 0;

    public DataSeeder()
    {
        var faker = new Faker<WeatherForecast>().CustomInstantiator(f =>
        {
            return new WeatherForecast(
                f.Date.Between(new DateTime(2024, 1, 1), new DateTime(2024, 12, 31)).Date,
                f.Random.Int(0, 30),
                f.PickRandom<SummaryEnum>().ToString()
            );
        });
        faker.Generate(30).ForEach(x => db.Add(currentId++, x));
    }
}
