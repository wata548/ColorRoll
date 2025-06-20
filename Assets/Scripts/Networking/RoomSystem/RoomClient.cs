using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Networking.RoomSystem {
    public class RoomClient: RoomBase {
        
        //==================================================||Constant 
        public const int Port = 51234;
        public const float WaitReceiveTime = 1.5f;
        private const int TimeOutCheckInterval = 500;
        
        //==================================================||Fields 
        private readonly Dictionary<string, (string ip, int port)> _maps;
        //it opened on constructor, closed when get AllowRequset
        private readonly UdpClient _receiveClient;

        //==================================================||Properties
        public List<(string, string)> FindRoom => _maps
            .Select(room => (room.Key, room.Value.ip))
            .ToList();

        public override string OtherPlayerIp { get; protected set; } = "";
        
        //==================================================||Constructors 
        public RoomClient() {
            _maps = new();
            _receiveClient = new(Port);
        }

        //==================================================||Methods 
        
        public void Refresh() {
            Send();
            Task.Run(Receive);
        }
        
        public void SelectOtherPlayer(string roomName) {
        
            if (!_maps.TryGetValue(roomName, out var host))
                return;
                    
            var sendClient = new UdpClient();
            var target = new IPEndPoint(IPAddress.Parse(host.ip), host.port);
        
            var data = new RoomInfo(RoomFindCommand.JoinRequest, GetIP(), Port);
            var rawData = ToByte(data);
            sendClient.Send(rawData, rawData.Length, target);
                    
            sendClient.Close();
                    
            //Wait till get Allow from host
            Task.Run(Receive);
        }

        private void Send() {
            
            _maps.Clear();
            var sendClient = new UdpClient();
            sendClient.EnableBroadcast = true;
            var sendEndPoint = new IPEndPoint(IPAddress.Broadcast, RoomHost.Port);
            
            var data = new RoomInfo(RoomFindCommand.RoomRequest, GetIP(), Port);
            var rawData = ToByte(data);
            sendClient.Send(rawData, rawData.Length, sendEndPoint);
            sendClient.Close();
        }
        
        private async Task Receive() {

            var sendTime = DateTime.Now;

            while ((DateTime.Now - sendTime).TotalSeconds <= WaitReceiveTime) {

                if (!string.IsNullOrWhiteSpace(OtherPlayerIp))
                    break;
                
                var receiveTask = _receiveClient.ReceiveAsync();
                var timeoutTask = Task.Delay(TimeOutCheckInterval);

                //check timeout
                var result = await Task.WhenAny(receiveTask, timeoutTask);
                if (result != receiveTask)
                    continue;

                var receiveRawData = receiveTask.Result.Buffer;
                var receiveData = ToData(receiveRawData);

                switch (receiveData.Command) {
                    case RoomFindCommand.RoomInfo:
                        _maps.TryAdd(receiveData.Name, (receiveData.Ip, receiveData.Port));
                        break;
                    case RoomFindCommand.Allow:
                        OtherPlayerIp = receiveData.Ip;
                        _receiveClient.Close();
                        break;           
                }
            }
        }
    }
}