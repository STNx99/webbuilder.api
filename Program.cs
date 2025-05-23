using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webbuilder.api.data;
using webbuilder.api.middleware;
using webbuilder.api.services;
using webbuilder.api.services.interfaces;
using webbuilder.api.repositories;
using webbuilder.api.repositories.interfaces;
using System.Security.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Configure HttpClient SSL/TLS settings
HttpClientHandler handler = new HttpClientHandler();
handler.SslProtocols = SslProtocols.Tls12 | SslProtocols.Tls13;
handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
handler.CheckCertificateRevocationList = false;

builder.Services.AddSingleton(handler);
builder.Services.AddHttpClient("ClerkClient")
    .ConfigurePrimaryHttpMessageHandler(() => handler);

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
     policy => { policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod(); });
});

// Configure JSON serialization options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });

builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
});

builder.Services.AddDbContext<ElementStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IElementRepository, ElementRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<ISettingsRepository, SettingsRepository>();
builder.Services.AddScoped<IImageRepository, ImageRepository>();

// Register services
builder.Services.AddScoped<IElementsService, ElementsService>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();
builder.Services.AddScoped<ISettingsService, SettingsService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();
app.UseCors("AllowAllOrigins");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseWhen(context =>
    (context.Request.Path.StartsWithSegments("/api/v1.0/elements") &&
     !context.Request.Path.StartsWithSegments("/api/v1.0/elements/public")) ||
    context.Request.Path.StartsWithSegments("/api/v1.0/projects") ||
    context.Request.Path.StartsWithSegments("/api/v1.0/images"), app =>
{
    app.UseUserAuthenticate();
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();