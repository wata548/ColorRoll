using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Networking.InGame;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking.RoomSystem {
    public class RoomClient: RoomBase {
        
        //==================================================||Constant 
        public const int Port = 51122;
        public const float WaitReceiveTime = 1.5f;
        private const int TimeOutCheckInterval = 500;
        
        //==================================================||Fields 
        private static Dictionary<string, (string ip, int port)> _maps;
        private static UdpClient _receiveClient;
        private static bool _isGameStart = false;
        
        public static bool IsInRoom { get; private set; } = false;
        public static bool IsOpen { get; private set; } = false;
        public static string Context { get; private set; } = "";
        
        //==================================================||Properties
        public List<(string, string)> FindRoom => _maps
            .Select(room => (room.Key, room.Value.ip))
            .ToList();

        public static string OtherPlayerIp { get; protected set; } = "";
        public static string RoomName { get; private set; } = "";
        
        //==================================================||Constructors 
        public RoomClient() {
            IsOpen = true;
            IsInRoom = false;
            if(_maps == null)
                _maps = new();
            if (_receiveClient == null) {
                
                _receiveClient = new(Port);
                Task.Run(Receive);
            }
        }

        //==================================================||Methods 

        public void SendData(string context) {
            
            var sendClient = new UdpClient();
            var target = new IPEndPoint(IPAddress.Parse(OtherPlayerIp), RoomHost.Port);
                    
            var data = new RoomInfo(RoomCommand.Data, GetIP(), Port, context);
            var rawData = ToByte(data);
            sendClient.Send(rawData, rawData.Length, target);
                                
            sendClient.Close();
        }
        
        public static void Start() {
            if (_isGameStart) {
                _isGameStart = false;
                //UdpManager.Start(OtherPlayerIp, false);
                SceneManager.LoadScene("Game");
            }
        }
        
        public void Close() {
            IsOpen = false;
        }

        public void Ready() {
            Send(OtherPlayerIp, RoomHost.Port, RoomCommand.Start);
        }
        public void Quit() {
            
            Send(OtherPlayerIp, RoomHost.Port, RoomCommand.Quit);
            Quited();
        }
        private void Quited() {
            IsInRoom = false;
            OtherPlayerIp = "";
        }
        
        public void Refresh() {
            SendFindRoomRequest();
        }
        
        public void RoomJoinRequest(string roomName) {

            if (!_maps.TryGetValue(roomName, out var host))
                return;
            
            Send(host.ip, host.port, RoomCommand.JoinRequest);
            RoomName = roomName;
        }

        private void Send(string ip, int port, RoomCommand command) {
            
            Debug.Log($"Send {command} to {ip} {IsOpen}");
            
            var sendClient = new UdpClient();
            var target = new IPEndPoint(IPAddress.Parse(ip), port);
                    
            var data = new RoomInfo(command, GetIP(), Port);
            var rawData = ToByte(data);
            sendClient.Send(rawData, rawData.Length, target);
                                
            sendClient.Close();
        }

        private void SendFindRoomRequest() {
            
            _maps.Clear();
            var sendClient = new UdpClient();
            sendClient.EnableBroadcast = true;
            var sendEndPoint = new IPEndPoint(IPAddress.Broadcast, RoomHost.Port);
            
            Debug.Log("Find");
            var data = new RoomInfo(RoomCommand.RoomRequest, GetIP(), Port);
            var rawData = ToByte(data);
            sendClient.Send(rawData, rawData.Length, sendEndPoint);
            sendClient.Close();
        }
        
        private Task Receive() {

            var target = new IPEndPoint(IPAddress.Any, Port);
            
            while (true) {

                Debug.Log($"ready to connet {IsInRoom}");
                var receiveRawData = _receiveClient.Receive(ref target);
                var receiveData = ToData(receiveRawData);
                Debug.Log($"Get data {receiveData.Command} {IsOpen}-{IsInRoom}");
                
                if (!IsOpen)
                    continue;
                
                switch (receiveData.Command) {
                    case RoomCommand.RoomInfo:
                        _maps.TryAdd(receiveData.Name, (receiveData.Ip, receiveData.Port));
                        break;
                    case RoomCommand.Allow:
                        Debug.Log($"enter {OtherPlayerIp}'s room");
                        if (string.IsNullOrWhiteSpace(OtherPlayerIp)) {

                            IsInRoom = true;
                            OtherPlayerIp = receiveData.Ip;
                        }
                        break;           
                    case RoomCommand.Quit:
                        Quited();
                        break;
                    case RoomCommand.Start:
                        if (!IsInRoom)
                            break;
                        _isGameStart = true;    
                        break;
                    case RoomCommand.Data:
                        Context = receiveData.Name;
                        break;
                }
            }

            return null;
        }
    }
}