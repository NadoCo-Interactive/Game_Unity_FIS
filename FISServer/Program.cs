var builder = WebApplication.CreateBuilder(args);

// Register SignalR services
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .SetIsOriginAllowed(_ => true); // Allow all origins
    });
});

var app = builder.Build();

// Map the SignalR hub
app.MapHub<GameHub>("/gameHub");

app.Run();
