namespace TimedChecker.Job.DI;

public static class SwaggerDI
{
    public static void EnableSwagger(this WebApplication app)
    {
        bool.TryParse(app.Configuration["EnableSwagger"], out var enabled);
        if (enabled)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}