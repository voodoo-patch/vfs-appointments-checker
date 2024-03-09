using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using TimedChecker.Job.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHealthChecks().Services
    .AddEndpointsApiExplorer()
    .AddApplicationServices()
    .AddQuartzJob(builder.Configuration)
    .AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapHealthChecks("/healthz", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.MapEndpoints();

app.Run();