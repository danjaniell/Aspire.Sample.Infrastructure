using System;
using Infrastructure.DB.InMemory.Entities;
using Infrastructure.DB.InMemory.Services;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.DB.InMemory.Endpoints;

public static class DatabaseEndpoints
{
    public static void MapDatabaseEndpoints(this WebApplication app)
    {
        DataSeeder dataSeeder = new();
        Dictionary<int, WeatherForecast> db = dataSeeder.db;

        app.MapGet(
                "/database",
                ([FromQuery] DateTime? startDate) =>
                {
                    if (startDate.HasValue)
                    {
                        return db.Values.Where(x => x.Date >= startDate.Value).ToArray();
                    }
                    else
                    {
                        return [.. db.Values];
                    }
                }
            )
            .Produces<WeatherForecast[]>(StatusCodes.Status200OK);

        app.MapPost(
                "/database",
                (WeatherForecast forecast) =>
                {
                    int id = AssignId(db);
                    db.Add(id, forecast);
                    return Results.Created($"/database/{id}", forecast);
                }
            )
            .Produces<int>(StatusCodes.Status201Created);

        app.MapDelete(
                "/database/{id}",
                (int id) =>
                {
                    if (db.Remove(id, out _))
                    {
                        return Results.NoContent();
                    }
                    return Results.NotFound();
                }
            )
            .Produces(StatusCodes.Status204NoContent);

        app.MapGet(
                "/database/{id:int}",
                (int id) =>
                {
                    if (db.TryGetValue(id, out WeatherForecast forecast))
                    {
                        if (forecast is not null)
                        {
                            return Results.Ok(forecast);
                        }
                    }
                    return Results.NotFound();
                }
            )
            .Produces<WeatherForecast>(StatusCodes.Status200OK);

        app.MapPut(
                "/database/{id}",
                (int id, WeatherForecast forecast) =>
                {
                    if (db.TryGetValue(id, out WeatherForecast oldForecast))
                    {
                        db[id] = forecast;
                        return Results.NoContent();
                    }
                    return Results.NotFound();
                }
            )
            .Produces(StatusCodes.Status204NoContent);

        app.MapGet("/database/maxId", () => db.Keys.Max());
    }

    private static int AssignId(Dictionary<int, WeatherForecast> db) => db.Keys.Max() + 1;
}
