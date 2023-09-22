using Hangfire;
using IBA.WebApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using IBA.WebApi.Controllers;
using StackExchange.Profiling.Storage;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.Hosting.Server;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHangfire(x => x.UseSqlServerStorage("Server=213.238.168.103;Database=IremBeyzaDB;User Id=iremBeyzaUser;Password=irem-beyza-06;"));
builder.Services.AddHangfireServer();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<Context>();
builder.Services.AddHttpContextAccessor();
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



builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
});

//PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
//        RateLimitPartition.GetConcurrencyLimiter(
//            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
//            factory: partition => new ConcurrencyLimiterOptions
//            {
//                PermitLimit = 1
//            }));

//PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
//    RateLimitPartition.GetFixedWindowLimiter(
//        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
//        factory: partition => new FixedWindowRateLimiterOptions
//        {
//            AutoReplenishment = true,
//            PermitLimit = 4,
//            QueueLimit = 1,
//            Window = TimeSpan.FromSeconds(60)
//        }));

 
builder.Services.AddRateLimiter(options =>
{
 
options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 5,
                QueueLimit = 2,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                Window = TimeSpan.FromMinutes(1)
            }));
});
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddFixedWindowLimiter("GetCountry", options =>
//    {
//        options.AutoReplenishment = true;
//        options.PermitLimit = 5;
//        options.Window = TimeSpan.FromMinutes(1);
//    });

//    options.AddFixedWindowLimiter("GetAllCountry", options =>
//    {
//        options.AutoReplenishment = true;
//        options.PermitLimit = 6;
//        options.Window = TimeSpan.FromMinutes(1);
//    });
//});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHangfireDashboard();

app.UseHttpsRedirection();
app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.UseMiniProfiler();

app.Run();
