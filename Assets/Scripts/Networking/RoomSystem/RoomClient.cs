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
        public const float WaitReceiveTime = 3f;
        private const int DefaultWaitTime = 500;
        
        //==================================================||Fields 
        private Dictionary<string, (string, int)> _maps = new();
        private UdpClient _receiveClient;

       //==================================================||Properties
       public List<(string, string)> FindedRoom => _maps
           .Select(room => (room.Key, room.Value.Item1))
           .ToList();
        
       //==================================================||Constructors 
        public RoomClient() {
            _receiveClient = new(Port);
        }

       //==================================================||Methods 
        public void Refresh() {
            Send();
            Task.Run(Receive);
        }
        
        private void Send() {
            
            _maps.Clear();
            var sendClient = new UdpClient();
            sendClient.EnableBroadcast = true;
            var sendEndPoint = new IPEndPoint(IPAddress.Broadcast, RoomHost.Port);
            
            var data = new RoomInfo(RoomFindCommand.Find, GetIP(), Port, "");
            var sendJson = JsonConvert.SerializeObject(data);
            var rawData = Encoding.UTF8.GetBytes(sendJson);
            sendClient.Send(rawData, rawData.Length, sendEndPoint);
            sendClient.Close();
        }
        
        private async Task Receive() {

            var sendTime = DateTime.Now;

            while ((DateTime.Now - sendTime).TotalSeconds <= WaitReceiveTime) {
                try {
                    var receiveTask = _receiveClient.ReceiveAsync();
                    var timeoutTask = Task.Delay(DefaultWaitTime);

                    var result = await Task.WhenAny(receiveTask, timeoutTask);
                    if (result != receiveTask)
                        continue;

                    var receiveRawData = receiveTask.Result.Buffer;
                    var receiveJson = Encoding.UTF8.GetString(receiveRawData);
                    var receiveData = JsonConvert.DeserializeObject<RoomInfo>(receiveJson);
                    if (receiveData.Command == RoomFindCommand.RoomInfo) {
                        _maps.TryAdd(receiveData.Name, (receiveData.Ip, receiveData.Port));
                    }
                }
                catch(SocketException e) {
                    Debug.LogError($"Network Exception: {e.Message}");
                }
            }
        }
    }

}