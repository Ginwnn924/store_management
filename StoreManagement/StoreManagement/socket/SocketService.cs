using Fleck;
using StoreManagement.Services;
using StoreManagement.Services.Impl;
using System.Drawing;
using System.IO;
using System.Text.Json;

namespace StoreManagement.socket
{
    public class SocketService : IHostedService
    {
        private WebSocketServer _server;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly WebSocketConnectionManager _manager;

        public SocketService(WebSocketConnectionManager manager, IServiceScopeFactory serviceScopeFactory)
        {
            _manager = manager;
            _serviceScopeFactory = serviceScopeFactory;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _server = new WebSocketServer("ws://0.0.0.0:8181");

            _server.Start(socket =>
            {
                string path = socket.ConnectionInfo.Path;
                string roomId = path.TrimStart('/');
                socket.OnOpen = () =>
                {
                    _manager.AddSocketToRoom(roomId, socket);
                    int size = _manager.GetConnectionCountInRoom(roomId);
                    Console.WriteLine($"Open connections {size}");
                };

                socket.OnClose = () =>
                {
                    _manager.RemoveSocketFromRoom(roomId, socket);
                    int size = _manager.GetConnectionCountInRoom(roomId);
                    Console.WriteLine($"Close connections {size}");
                };

                socket.OnMessage = async message =>
                {
                    Console.WriteLine(message);
                    using var scope = _serviceScopeFactory.CreateScope();
                    var productService = scope.ServiceProvider.GetRequiredService<IProductService>();

                    // Gọi service
                    var response = await productService.GetProductByBarcodeAsync(message);

                    // Thêm options để giữ nguyên tiếng Việt
                    var options = new JsonSerializerOptions
                    {
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                        WriteIndented = true // Optional: format đẹp hơn
                    };

                    string jsonString = JsonSerializer.Serialize(response, options);

                    await _manager.BroadcastToRoom(roomId, jsonString, socket);
                    //await _manager.BroadcastAsync($"Server: {message}");
                };
            });

            //_logger.LogInformation("WebSocket Server started on ws://localhost:8181");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _server?.Dispose();
            return Task.CompletedTask;
        }
    }
}
