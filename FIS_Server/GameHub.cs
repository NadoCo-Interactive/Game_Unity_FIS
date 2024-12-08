using Microsoft.AspNetCore.SignalR;

public class GameHub : Hub
{
    public override Task OnConnectedAsync()
    {
        // Get the connection ID of the connected client
        var connectionId = Context.ConnectionId;

        // .. broadcast to all clients to spawn the player object
        Clients.Others.SendAsync("PlayerConnected", connectionId);

        return base.OnConnectedAsync();
    }
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        // Get the connection ID of the connected client
        var connectionId = Context.ConnectionId;

        // .. broadcast to all clients to delete the player object
        Clients.Others.SendAsync("PlayerDisconnected", connectionId);

        return base.OnDisconnectedAsync(exception);
    }
    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
}
