using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Networking.RoomSystem {
    
    public class RoomHost: RoomBase {

        public const int Port = 54321;
        private readonly string _roomName;
        private UdpClient _receiverClient;
        
        public RoomHost() {
            
            Task.Run(SendRoomInfo);
            _receiverClient = new(Port);
        }
        
        private Task SendRoomInfo() {
            var remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);

            while (true) {

                var rawData = _receiverClient.Receive(ref remoteEndPoint);
                var receiveJson = Encoding.UTF8.GetString(rawData);
                var receiveData = JsonConvert.DeserializeObject<RoomInfo>(receiveJson);
                if(receiveData.Command != RoomFindCommand.Find)
                    continue;

                var sendClient = new UdpClient();
                var data = new RoomInfo(RoomFindCommand.RoomInfo, GetIP() , Port, _roomName);
                
                var sendJson = JsonConvert.SerializeObject(data);
                var sendData = Encoding.UTF8.GetBytes(sendJson);
                var target = new IPEndPoint(IPAddress.Parse(receiveData.Ip), receiveData.Port);
                sendClient.SendAsync(sendData, sendData.Length, target);
                sendClient.Close();
            }
        }
    }
}