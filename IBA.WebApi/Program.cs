using IBA.WebApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using IBA.WebApi.Controllers;
using StackExchange.Profiling.Storage;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<Context>();
builder.Services.AddMemoryCache();
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/mini-profiler";
    options.Storage = new SqlServerStorage("Server=213.238.168.103;Database=IremBeyzaDB;User Id=iremBeyzaUser;Password=irem-beyza-06;MultipleActiveResultSets=true", "MiniProfilers", "MiniProfilerTimings", "MiniProfilerClientTimings");
    options.IgnoredPaths.Add("/swagger");
    options.ColorScheme = StackExchange.Profiling.ColorScheme.Dark;
    
    options.TrackConnectionOpenClose = true; 
    options.SqlFormatter = new StackExchange.Profiling.SqlFormatters.InlineFormatter();
    options.UserIdProvider = (request) => request.HttpContext.User.Identity.Name;
}).AddEntityFramework();

builder.Services.AddSwaggerGen();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseMiniProfiler();

app.Run();
