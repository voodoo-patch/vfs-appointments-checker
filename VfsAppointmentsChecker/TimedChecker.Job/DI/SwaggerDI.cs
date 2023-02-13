﻿namespace TimedChecker.Job.DI;

public static class SwaggerDI
{
    public static void EnableSwagger(this WebApplication app)
    {
        Boolean.TryParse(app.Configuration["EnableSwagger"], out bool enabled);
        if (enabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}