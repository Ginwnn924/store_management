using Fleck;
using System.Collections.Concurrent;

namespace StoreManagement.socket
{
    public class WebSocketConnectionManager
    {
        private readonly ConcurrentDictionary<string, List<IWebSocketConnection>> _rooms = new();

        public void AddSocketToRoom(string roomId, IWebSocketConnection socket)
        {
            _rooms.AddOrUpdate(
                roomId,
                new List<IWebSocketConnection> { socket },
                (key, existingList) =>
                {
                    existingList.Add(socket);
                    return existingList;
                }
            );
        }

        public void RemoveSocketFromRoom(string roomId, IWebSocketConnection socket)
        {
            if (_rooms.TryGetValue(roomId, out var sockets))
            {
                sockets.Remove(socket);

                // Xóa room nếu không còn ai
                if (sockets.Count == 0)
                {
                    _rooms.TryRemove(roomId, out _);
                }
            }
        }

        public async Task BroadcastToRoom(string roomId, string message, IWebSocketConnection excludeSocket = null)
        {
            if (_rooms.TryGetValue(roomId, out var sockets))
            {
                foreach (var socket in sockets.ToList())
                {
                    // Skip socket cần exclude
                    if (socket == excludeSocket)
                        continue;

                    try
                    {
                        await socket.Send(message);
                    }
                    catch
                    {
                        sockets.Remove(socket);
                    }
                }
            }
        }

        public async Task BroadcastToAll(string message)
        {
            foreach (var room in _rooms.Values)
            {
                foreach (var socket in room.ToList())
                {
                    try
                    {
                        await socket.Send(message);
                    }
                    catch { }
                }
            }
        }

        public int GetRoomCount() => _rooms.Count;

        public int GetConnectionCountInRoom(string roomId)
        {
            return _rooms.TryGetValue(roomId, out var sockets) ? sockets.Count : 0;
        }

        public int GetTotalConnectionCount()
        {
            return _rooms.Values.Sum(sockets => sockets.Count);
        }
    
    }
}

