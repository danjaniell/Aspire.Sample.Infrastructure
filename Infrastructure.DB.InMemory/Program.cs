using Infrastructure.DB.InMemory.Endpoints;
using Infrastructure.DB.InMemory.Services;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder
    .Services.AddOpenTelemetry()
    .WithMetrics(metrics =>
    {
        metrics
            .AddRuntimeInstrumentation()
            .AddMeter(
                "Microsoft.AspNetCore.Hosting",
                "Microsoft.AspNetCore.Server.Kestrel",
                "System.Net.Http"
            );
    })
    .WithTracing(tracing =>
    {
        tracing.AddAspNetCoreInstrumentation().AddHttpClientInstrumentation();
    });

var useOtlpExporter = !string.IsNullOrWhiteSpace(
    builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]
);
if (useOtlpExporter)
{
    builder.Services.AddOpenTelemetry().UseOtlpExporter();
}

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapDatabaseEndpoints();

app.Run();
