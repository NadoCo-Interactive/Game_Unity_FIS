using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public override Task OnConnectedAsync()
    {
        Console.WriteLine("Client connected: " + Context.ConnectionId);
        return base.OnConnectedAsync();
    }
    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine(user + " says: " + message);
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}