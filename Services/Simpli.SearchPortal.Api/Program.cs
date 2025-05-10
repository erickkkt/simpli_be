using Microsoft.AspNetCore.Localization;
using Simpli.SearchPortal.Api.Helper;

using ConfigurationManager = Microsoft.Extensions.Configuration.ConfigurationManager;
using System.Text.Json.Serialization;
using Simpli.SearchPortal.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                     .AddEnvironmentVariables();

builder.Logging.AddDebug()
               .AddConsole();

ConfigureServices(builder.Configuration, builder.Environment);

var app = builder.Build();

Configure(builder.Configuration, builder.Environment);

try
{
    app.Run();
}
catch (Exception ex)
{
    var logger = app.Services.GetRequiredService<ILogger<Program>>();
    logger.LogCritical(ex, "Unhandled exception occurred while running the application.");

    throw;
}

void ConfigureServices(ConfigurationManager configuration, IWebHostEnvironment environment)
{
    builder.Services.AddOptions();
    builder.Services.AddLogging();

    builder.Services.AddApplicationInsightsTelemetry();

    builder.Services.AddAuthorization();
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin()
                .WithExposedHeaders("Location")
        );
    });

    builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        options.DefaultRequestCulture = new RequestCulture("en-AU");
    });

    if (!environment.IsProduction())
    {
        builder.Services.AddSwaggerGen();
    }

    builder.Services.AddBaseSettings(configuration);
    builder.Services.AddServices(configuration);
    builder.Services.AddControllers()
                   .AddJsonOptions(options =>
                   {
                       options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                   });
}

void Configure(ConfigurationManager configuration, IWebHostEnvironment environment)
{
    app.AddSecurityHeaders();
    app.UseHsts();

    if (environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }

    app.UseHttpsRedirection();

    app.UseCors("CorsPolicy");

    app.UseMiddleware<ExceptionHandlingMiddleware>();

    app.UseAuthorization();

    app.MapControllers();
}
