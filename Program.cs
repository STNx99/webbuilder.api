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
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IElementsService, ElementsService>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();
app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// app.UseUserAuthenticate();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();