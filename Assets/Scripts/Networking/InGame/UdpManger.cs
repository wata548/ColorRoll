using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Networking.RoomSystem;
using Newtonsoft.Json;
using UnityEngine;

namespace Networking.InGame {
    public static class UdpManager {
        private const int Port2 = 51234;
        private const int Port1 = 54321;
        private static UdpClient _receiver = null;
        private static UdpClient _sender = null;
        private static IPEndPoint _sendTarget;
        private static IPEndPoint _receiveTarget;
        private static IData _updateData = null;
        private static bool _isFirst = true;

        public static Task Process;

        public static IData CurData =>
            _updateData;
        
        public static void Start(string otherPlayerIp) {

            int Port = NetworkManager.Instance.IsHost ? Port1 : Port2;
            int PortAnother = NetworkManager.Instance.IsHost ? Port2 : Port1;
            Debug.Log(Port);
            
            _receiveTarget = new(IPAddress.Any, Port);
            _receiver = new(_receiveTarget);
         
            _sender = new();
            _sendTarget = new(IPAddress.Parse(otherPlayerIp), PortAnother);

            if (_isFirst)
                Process = Task.Run(Logic);
            _isFirst = false;
        }

        public static void Send<T>(T data) where T: IData {
         
            
            var sendData = new SocketData(data); 
            var rawData = JsonConvert.SerializeObject(sendData);
            var byteData = Encoding.UTF8.GetBytes(rawData);
            _sender.Send(byteData, byteData.Length, _sendTarget);
        }
        
        private static Task Logic() {
            
            while (true) {
                Debug.Log($"ReceiveReady {_sendTarget.Address} {_receiveTarget.Address} +  {_receiveTarget.Port} - {_sendTarget.Port}");
                var byteData = _receiver.Receive(ref _receiveTarget);
                var rawData = Encoding.UTF8.GetString(byteData);
                var data = JsonConvert.DeserializeObject<SocketData>(rawData);
                Debug.Log($"Receive data {data.DataType}");

                _updateData = data.DataType switch {
                    DataType.Input => JsonConvert.DeserializeObject<InputData>(data.Data),
                    DataType.CurState => JsonConvert.DeserializeObject<InputData>(data.Data),
                    _ => null
                };
            }

            return null;
        }

    }
}