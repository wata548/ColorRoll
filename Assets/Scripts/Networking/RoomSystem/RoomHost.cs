using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Networking.RoomSystem {
    
    public class RoomHost: RoomBase {

        //==================================================||Constant      
        public const int Port = 54321;
        
        //==================================================||Fields 
        private readonly string _roomName;
        
        //it opened on constructor, closed when get JoinRequest
        private readonly UdpClient _receiverClient;
        
        //==================================================||Properties 
        public override string OtherPlayerIp { get; protected set; } = "";
        
        //==================================================||Constructors 
        public RoomHost(string roomName) {

            _roomName = roomName;
            _receiverClient = new(Port);
            Task.Run(Udp);
        }
        
        //==================================================||Methods 


        private Task Udp() {
            var remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);

            while (true) {

                if (!string.IsNullOrWhiteSpace(OtherPlayerIp))
                    break;
                
                //receive
                Debug.Log("Wait for receive");
                var rawData = _receiverClient.Receive(ref remoteEndPoint);
                var receiveData = ToData(rawData);
                Debug.Log("Receive");
                
                //after receive
                switch (receiveData.Command) {
                    case RoomFindCommand.RoomRequest:
                        Send(receiveData.Ip, receiveData.Port, RoomFindCommand.RoomInfo);
                        break;
                    case RoomFindCommand.JoinRequest:
                        OtherPlayerIp = receiveData.Ip;
                        Send(receiveData.Ip, receiveData.Port, RoomFindCommand.Allow);
                        _receiverClient.Close();
                        break;
                    default:
                        continue;
                }        
            }
            _receiverClient.Close();
            
            return null;
        }

        private void Send(string ip, int port, RoomFindCommand command) {
            
            var sendClient = new UdpClient();
            var target = new IPEndPoint(IPAddress.Parse(ip), port);
                        
            var data = new RoomInfo(command, GetIP() , Port, _roomName);
            var sendData = ToByte(data);
                        
            sendClient.Send(sendData, sendData.Length, target);
                        
            sendClient.Close();
        }
    }
}