using System.Net.WebSockets;
using System.Text;

namespace cat_cafe.WeSo
{
    public class WebSocketHandler
    {
        private readonly List<WebSocket> _sockets;

        public WebSocketHandler(List<WebSocket> sockets)
        {
            _sockets = sockets;
        }

        public async Task BroadcastMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            foreach (var socket in _sockets)
            {
                if (socket.State == WebSocketState.Open)
                {
                    await socket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
                }
            }
        }
    }

}
