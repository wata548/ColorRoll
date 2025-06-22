using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Networking.InGame {
    public static class UdpManager {

        private const int Port = 51122;
        private static UdpClient _receiver = null;
        private static UdpClient _sender = null;
        private static IPEndPoint _sendTarget;
        private static IPEndPoint _receiveTarget;
        private static SocketData _updateData = null;

        public static SocketData CurData =>
            _updateData;
        
        private static void Start(string otherPlayerIp) {
            
            _receiver ??= new(Port);
         
            _sender ??= new();
            _receiveTarget ??= new(IPAddress.Any, Port);
            _sendTarget = new(IPAddress.Parse(otherPlayerIp), Port);
        }

        private static void Send(SocketData data) {
            
            var rawData = JsonConvert.SerializeObject(data);
            var byteData = Encoding.UTF8.GetBytes(rawData);
            _sender.Send(byteData, byteData.Length, _sendTarget);
        }
        
        private static Task Logic() {
            
            while (true) {
                
                var byteData = _receiver.Receive(ref _receiveTarget);
                var rawData = Encoding.UTF8.GetString(byteData);
                _updateData = JsonConvert.DeserializeObject<SocketData>(rawData);
            }
        }

    }
}