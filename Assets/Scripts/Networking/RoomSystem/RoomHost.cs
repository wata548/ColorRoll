using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Networking.RoomSystem {
    
    public class RoomHost: RoomBase {

        public const int Port = 51234;
        private readonly string _roomName;
        private UdpClient _receiverClient;
        
        public RoomHost(string roomName) {

            _roomName = roomName;
            _receiverClient = new(Port);
            Task.Run(SendRoomInfo);
        }
        
        private Task SendRoomInfo() {
            var remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);

            while (true) {

                var rawData = _receiverClient.Receive(ref remoteEndPoint);
                var receiveJson = Encoding.UTF8.GetString(rawData);
                var receiveData = JsonConvert.DeserializeObject<RoomInfo>(receiveJson);
                Debug.Log("Receive");
                
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