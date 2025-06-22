using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CSVData;
using Newtonsoft.Json;
using UnityEngine;

namespace Networking.InGame {
    /*public static class UdpManager {
        private const int Port1 = 51234;
        private const int Port2 = 54321;
        
        private static UdpClient _receiver;
        private static UdpClient _sender;
        private static IPEndPoint _receivePort;
        private static IPEndPoint _sendTarget;
        private static bool _isFirst = true;

        public static string CurData { get; private set; } = "";
        
        public static void Start(string ip, bool isHost) {

            Debug.Log($"Open Port {_isFirst}");
            
            if (!_isFirst)
                return;

            _isFirst = false;

            if (isHost) {
                Debug.Log("I'm host");
                _receivePort = new(IPAddress.Any, Port1);
                _sendTarget = new(IPAddress.Parse(ip), Port2);
            }
            else {
                Debug.Log("I'm client");
                _receivePort = new(IPAddress.Any, Port2);
                _sendTarget = new(IPAddress.Parse(ip), Port1);
            }

            _sender = new();
            _receiver = new(_receivePort);

            Task.Run(Receive);
        }

        public static void Send(IData sendData) {

            Debug.Log("Send");
            var data = Encoding.UTF8.GetBytes(sendData.ToString());
            _sender.Send(data, data.Length, _sendTarget);
        }
        private static Task Receive() {
            while (true) {
                Debug.Log("Wait for Receive!");
                var receiveData = _receiver.Receive(ref _receivePort);
                Debug.Log("Receive!!!");
                CurData = Encoding.UTF8.GetString(receiveData);
                Debug.Log($"ReceiveData: {CurData}!!!!!!!!!!!!");
                
            }
        }
    }*/
}