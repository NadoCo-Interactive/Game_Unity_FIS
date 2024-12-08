var builder = WebApplication.CreateBuilder(args);

// Register SignalR services
builder.Services.AddSignalR();

var app = builder.Build();

// Map the SignalR hub
app.MapHub<GameHub>("/gameHub");

app.Run();