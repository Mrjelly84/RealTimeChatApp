using Microsoft.AspNetCore.SignalR.Client;
using Shared.Models;

public class ChatService
{
    private HubConnection? _hubConnection;
    private readonly SemaphoreSlim _startLock = new(1, 1);
    public event Action<ChatMessage>? OnMessageReceived;

    public ChatService()
    {
        // Don't initialize connection here
    }

    public async Task StartAsync()
    {
        await _startLock.WaitAsync();
        try
        {
            if (_hubConnection == null)
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:5143/chathub")
                    .WithAutomaticReconnect()
                    .Build();

                _hubConnection.On<ChatMessage>("ReceiveMessage", (message) =>
                {
                    OnMessageReceived?.Invoke(message);
                });
            }

            if (_hubConnection.State != HubConnectionState.Disconnected)
            {
                return;
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
        finally
        {
            _startLock.Release();
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