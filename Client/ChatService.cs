using Microsoft.AspNetCore.SignalR.Client;
using Shared.Models;

public class ChatService
{
    private HubConnection? _hubConnection;
    public event Action<ChatMessage>? OnMessageReceived;

    public ChatService()
    {
        // Don't initialize connection here
    }

    public async Task StartAsync()
    {
        if (_hubConnection == null)
        {
            // Use the correct URL - this should be your server's address
            _hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5157/chathub") // Make sure this matches your server port
                .WithAutomaticReconnect()
                .Build();
                
            _hubConnection.On<ChatMessage>("ReceiveMessage", (message) =>
            {
                OnMessageReceived?.Invoke(message);
            });
        }

        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine("SignalR connected successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
        }
    }

    public async Task SendMessage(ChatMessage message)
    {
        if (_hubConnection != null && _hubConnection.State == HubConnectionState.Connected)
        {
            await _hubConnection.SendAsync("SendMessage", message);
        }
        else
        {
            Console.WriteLine("SignalR connection is not active");
        }
    }

    public void Dispose()
    {
        _hubConnection?.StopAsync();
        _hubConnection?.DisposeAsync();
    }
}