using System;
using System.Net.Sockets;
using InputSystem;
using MapInfo;
using Networking.RoomSystem;
using Player;
using Test;
using UnityEngine;

namespace Networking.InGame {
    public class GameManager: MonoBehaviour {

        [Header("Host")] 
        [SerializeField] private CalculatePlayer _calculatePlayer1;
        [SerializeField] private CalculatePlayer _calculatePlayer2; 
        [Space]
        [Header("Client")] 
        [SerializeField] private InputPlayer _inputPlayer1;
        [SerializeField] private InputPlayer _inputPlayer2;

        private bool _isHost = false;
        private PlayerBase _hostPlayer;
        private PlayerBase _clientPlayer;
        private InputControl _input;

        private void Start() {
            _isHost = NetworkManager.Instance.IsHost;
            
            BlockGenerator.Instance.SetUp();
            if (_isHost) {
                _hostPlayer = Instantiate(_calculatePlayer1);
                _clientPlayer = Instantiate(_calculatePlayer2);
            }
            else {
                _hostPlayer = Instantiate(_inputPlayer1);
                _clientPlayer = Instantiate(_inputPlayer2);
            }

            _hostPlayer.SetHost();
            _clientPlayer.SetHost(false);
            
            _input = new("DefaultInput.csv");
         
            FloorDrawer.Instance.SetTarget(_hostPlayer.gameObject, _clientPlayer.gameObject);
        }


        private float _lastSend = 0;
        private const float SendInterval  = 0.1f;
        private void Update() {

            var input = new InputData(CameraFocus.CurRotation, _input);
            var context = _isHost ? RoomHost.Context : RoomClient.Context;
            
            Debug.Log(context);
            if (!string.IsNullOrWhiteSpace(context)) {
                
                if (_isHost) {
                    _clientPlayer.Apply(context);
                    _hostPlayer.Apply(input.ToString());
                }
                else {

                    _hostPlayer.Apply(context);
                    BlockGenerator.Instance.Apply(context);    
                    if (Time.time - _lastSend >= SendInterval) {
                        _lastSend = Time.time;
                        _clientPlayer.Apply(context);
                    }
                    
                }
            }
            
            if (_isHost) {
                
                var curData = new GameData(
                    (_hostPlayer as CalculatePlayer)!.FSM,
                    (_clientPlayer as CalculatePlayer)!.FSM
                );

                (NetworkManager.Instance.Room as RoomHost)!.SendData(curData.ToString());
                //UdpManager.Send(curData);
            }
            else {
                
                (NetworkManager.Instance.Room as RoomClient)!.SendData(input.ToString());
                //UdpManager.Send(input);
            }
        }
    }
}