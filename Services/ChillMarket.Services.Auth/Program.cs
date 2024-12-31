
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "ChillMarket.Services.Auth", Version = "v1", Description = "Авторизация и регистрация пользователей" });
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

var app = builder.Build();

app.UsePathBase("/auth");
app.UseRouting();
app.UseCors();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(
        options =>
        {
            options.RoutePrefix = "";
            options.SwaggerEndpoint("/auth/swagger/v1/swagger.json", "ChillMarket.Services.Auth");
        }
    );
}

app.MapGet("/login", () => "Вы вошли в систему");
app.MapGet("/register", () => "Вы зарегистрировались в системе");

app.Run();