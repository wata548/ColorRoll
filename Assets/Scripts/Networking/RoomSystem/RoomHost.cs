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
        public readonly string RoomName;
       
        private static UdpClient _receiveClient = null;

        public static bool IsOpen { get; private set; } = false;
        
        //==================================================||Properties 
        public override string OtherPlayerIp { get; protected set; } = "";
        
        //==================================================||Constructors 
        public RoomHost(string roomName) {

            IsOpen = true;
            Debug.Log("T");
            RoomName = roomName;
            if (_receiveClient == null) {
                
                _receiveClient = new(Port);
                Task.Run(Udp);
            }
        }
        
        //==================================================||Methods 
        public void Quit() {

            IsOpen = false;
            Debug.Log("F");
            if (!string.IsNullOrWhiteSpace(OtherPlayerIp)) {
                
                Send(OtherPlayerIp, RoomClient.Port, RoomCommand.Quit);
                OtherPlayerIp = "";
            }
        }

        private Task Udp() {
            var remoteEndPoint = new IPEndPoint(IPAddress.Any, Port);

            while (true) {

                //receive
                Debug.Log("Wait for receive");
                var rawData = _receiveClient.Receive(ref remoteEndPoint);
                var receiveData = ToData(rawData);
                Debug.Log($"Receive {receiveData.Command} ({IsOpen})");
                
                if(!IsOpen)
                    continue;
                
                //after receive
                switch (receiveData.Command) {
                    case RoomCommand.RoomRequest:
                        Send(receiveData.Ip, receiveData.Port, RoomCommand.RoomInfo);
                        break;
                    case RoomCommand.JoinRequest:
                        SelectOtherPlayer(receiveData);
                        break;
                    case RoomCommand.Quit:
                        OtherPlayerIp = "";
                        break;
                    default:
                        continue;
                }        
            }
            
            return null;
        }

        private void SelectOtherPlayer(RoomInfo data) {

            if (!string.IsNullOrWhiteSpace(OtherPlayerIp))
                return;
            
            OtherPlayerIp = data.Ip;
            Send(data.Ip, data.Port, RoomCommand.Allow);
        }
        
        private void Send(string ip, int port, RoomCommand command) {
            
            Debug.Log($"Send {command} to {ip}");
            var sendClient = new UdpClient();
            var target = new IPEndPoint(IPAddress.Parse(ip), port);
                        
            var data = new RoomInfo(command, GetIP() , Port, RoomName);
            var sendData = ToByte(data);
                        
            sendClient.Send(sendData, sendData.Length, target);
                        
            sendClient.Close();
        }
    }
}