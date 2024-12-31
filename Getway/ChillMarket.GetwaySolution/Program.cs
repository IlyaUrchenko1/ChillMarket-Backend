using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ChillMarket.GetwaySolution", Version = "v1", Description = "Шлюз для микросервисов ChillMarket" });
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Добавляем конфигурацию обратного прокси из appsettings.json
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            options.RoutePrefix = "";
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ChillMarket.GetwaySolution");
            options.SwaggerEndpoint("/auth/swagger/v1/swagger.json", "ChillMarket.Services.Auth");
        }
    );
}

app.UseCors();

// Используем YARP для проксирования запросов
app.MapReverseProxy();

app.MapGet("/", () => "ChillMarket API Gateway");

app.Run();
