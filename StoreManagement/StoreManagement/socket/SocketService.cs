using Fleck;
using System.Drawing;
using System.IO;

namespace StoreManagement.socket
{
    public class SocketService : IHostedService
    {
        private WebSocketServer _server;
        private readonly WebSocketConnectionManager _manager;

        public SocketService(WebSocketConnectionManager manager)
        {
            _manager = manager;
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
                    _manager.BroadcastToRoom(roomId, message, socket);
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
