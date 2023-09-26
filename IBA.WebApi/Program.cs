using Hangfire;
using IBA.WebApi.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Profiling.Storage;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using IBA.WebApi.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

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
var rsa = RSA.Create();
rsa.ImportRSAPublicKey(Convert.FromBase64String(builder.Configuration["JwtTokenOptions:PublicKey"]), out _);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opts =>
    {
        //byte[] signingKey = Encoding.UTF8
        // .GetBytes(builder.Configuration.GetSection("JwtTokenOptions")["SigningKey"]);

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidIssuer = builder.Configuration.GetSection("JwtTokenOptions")["Issuer"],
            ValidAudience = builder.Configuration.GetSection("JwtTokenOptions")["Audience"],
            IssuerSigningKey = new RsaSecurityKey(rsa)
        };
    });
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AspNetJWT", Version = "1.0" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description =
  "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\""
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = 429;
});




builder.Services.AddRateLimiter(options =>
{

    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    RateLimitPartition.GetFixedWindowLimiter(
        partitionKey: httpContext.User.Identity?.Name ?? httpContext.Request.Headers.Host.ToString(),
        factory: partition => new FixedWindowRateLimiterOptions
        {
            AutoReplenishment = true,
            PermitLimit = 10,
            QueueLimit = 10,
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            Window = TimeSpan.FromMinutes(1)
        }));
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHangfireDashboard();

app.UseHttpsRedirection();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseMiniProfiler();

app.Run();
