using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webbuilder.api.data;
using webbuilder.api.middleware;
using webbuilder.api.services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
     policy => { policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod(); });
});
builder.Services.AddControllers();

builder.Services.AddDbContext<ElementStoreContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IElementsService, ElementsService>();
builder.Services.AddScoped<IProjectsService, ProjectsService>();
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
    context.Request.Path.StartsWithSegments("/api/v1.0/elements") ||
    context.Request.Path.StartsWithSegments("/api/v1.0/projects"), app =>
{
    app.UseUserAuthenticate();
});

app.UseHttpsRedirection();
app.MapControllers();
app.Run();