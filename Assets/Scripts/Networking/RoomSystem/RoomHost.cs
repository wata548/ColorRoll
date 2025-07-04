﻿using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Networking.InGame;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking.RoomSystem {
    
    public class RoomHost: RoomBase {

        //==================================================||Constant      
        public const int Port = 51123;
        
        //==================================================||Fields 
        public static string RoomName;
       
        private static UdpClient _receiveClient = null;

        public static bool IsOpen { get; private set; } = false;
        public static string Context { get; private set; } = "";
        
        //==================================================||Properties 
        public static string OtherPlayerIp { get; private set; } = "";
        public static bool OtherPlayerReady { get; private set; } = false;
        
        //==================================================||Constructors 
        public RoomHost(string roomName) {

            IsOpen = true;
            OtherPlayerReady = false;
            OtherPlayerIp = "";
            
            RoomName = roomName;
            if (_receiveClient == null) {
                
                _receiveClient = new(Port);
                Task.Run(Udp);
            }
        }
        
        //==================================================||Methods 

        public void SendData(string context) {
            var sendClient = new UdpClient();
            var target = new IPEndPoint(IPAddress.Parse(OtherPlayerIp), RoomClient.Port);
                        
            var data = new RoomInfo(RoomCommand.Data, GetIP() , Port, context);
            var sendData = ToByte(data);
                        
            sendClient.Send(sendData, sendData.Length, target);
                        
            sendClient.Close();   
        }
        
        public void Start() {
            
            if(!OtherPlayerReady)
                return;

            //IsOpen = false;
            Send(OtherPlayerIp, RoomClient.Port, RoomCommand.Start);
            //UdpManager.Start(OtherPlayerIp, true);
            SceneManager.LoadScene("Game");
        }
        
        public void Quit() {

            IsOpen = false;
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
                        OtherPlayerReady = false;
                        OtherPlayerIp = "";
                        break;
                    case RoomCommand.Start:
                        OtherPlayerReady = !OtherPlayerReady;
                        break;
                    case RoomCommand.Data:
                        Context = receiveData.Name;
                        break;
                    default:
                        continue;
                }        
            }
            
            return null;
        }

        private void SelectOtherPlayer(RoomInfo data) {

            //if already other player exist
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