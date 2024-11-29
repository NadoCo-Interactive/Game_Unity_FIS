var builder = WebApplication.CreateBuilder(args);

// Add services to the container (before calling Build())
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});

var app = builder.Build();

// Configure middleware and routing
app.UseWebSockets();
app.UseRouting();

app.MapGet("/", () => "Hello World!");
app.MapHub<ChatHub>("/chatHub");

app.Run();
