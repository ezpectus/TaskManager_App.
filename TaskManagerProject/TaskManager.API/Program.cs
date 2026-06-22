using Asp.Versioning;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;
using TaskManager.Infrastucture.DepenInjection;
using TaskManager.Infrastucture.Persistence;
using TaskManager.Infrastucture.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/taskmanager-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<TaskManager.Application.Services.TaskService>();
        fv.ImplicitlyValidateChildProperties = true;
    });

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TaskManager API",
        Version = "v1",
        Description = "TaskManager API v1.6 — JWT auth with refresh tokens, task CRUD, subtasks, comments, activity logs, user management"
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            Array.Empty<string>()
        }
    });
});

builder.Services.AddInfrastructure(builder.Configuration);

var jwtKey = builder.Configuration["Jwt:Key"] ?? "TaskManager_DefaultSecretKey_2026_Min32Chars!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "TaskManager";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "TaskManager";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddHealthChecks();

builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy("api", context =>
        RateLimitPartition.GetTokenBucketLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "anonymous",
            _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 100,
                TokensPerPeriod = 100,
                ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

builder.Services.AddResponseCaching();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
        var ex = exceptionHandlerPathFeature?.Error;

        var statusCode = ex switch
        {
            ArgumentException => 400,
            UnauthorizedAccessException => 401,
            InvalidOperationException => 404,
            _ => 500
        };

        context.Response.StatusCode = statusCode;
        var response = new
        {
            error = new
            {
                code = statusCode,
                message = statusCode == 500 ? "Internal server error" : ex?.Message
            }
        };
        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    });
});

app.UseCors();
app.UseResponseCaching();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        await DbSeeder.SeedAsync(db);
    }
    catch (Exception ex)
    {
        Log.Logger.Error(ex, "An error occurred while seeding the database");
    }
}

app.Run();