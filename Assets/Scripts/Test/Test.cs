using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Networking {

    public class Test : MonoBehaviour {
        private void Awake() {
            #if UNITY_EDITOR
            NetWorkingTest.Start("127.0.0.1", true);
            #else
            NetWorkingTest.Start("127.0.0.1", false);
            #endif
        }

        private void Update() {
            NetWorkingTest.Send(Random.Range(0, 10).ToString());
        }
    }
    
    public static class NetWorkingTest {

        private const int Port1 = 51234;
        private const int Port2 = 54321;
        
        private static string _otherPlayerIp = "127.0.0.1";
        private static UdpClient _receiver;
        private static UdpClient _sender;
        private static IPEndPoint _receivePort;
        private static IPEndPoint _sendTarget;
        private static bool _isFirst = true;
        
        public static void Start(string ip, bool isHost) {

            if (!_isFirst)
                return;

            _isFirst = false;

            _otherPlayerIp = ip;
            if (isHost) {
                _receivePort = new(IPAddress.Any, Port1);
                _sendTarget = new(IPAddress.Parse(ip), Port2);
            }
            else {
                _receivePort = new(IPAddress.Any, Port2);
                _sendTarget = new(IPAddress.Parse(ip), Port1);
            }

            _sender = new();
            _receiver = new(_receivePort);

            Task.Run(Receive);
        }

        public static void Send(string text) {
            var data = Encoding.UTF8.GetBytes(text);
            _sender.Send(data, data.Length, _sendTarget);
        }
        private static Task Receive() {
            while (true) {
                var receiveData = _receiver.Receive(ref _receivePort);
                var data = Encoding.UTF8.GetString(receiveData);
                Debug.Log($"Send data: {data}");
            }
        }
    }
}