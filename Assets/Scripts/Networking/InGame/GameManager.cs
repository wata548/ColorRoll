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
            
            if (_isHost) {
                _hostPlayer = Instantiate(_calculatePlayer1);
                _clientPlayer = Instantiate(_calculatePlayer2);
                BlockGenerator.Instance.SetUp();
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

        private void Update() {

            var input = new InputData(CameraFocus.CurRotation, _input);
            
            if (UdpManager.CurData != null) {
                if (_isHost) {
                    _clientPlayer.Apply(UdpManager.CurData);
                    _hostPlayer.Apply(input);
                }
                else {
                    _hostPlayer.Apply(UdpManager.CurData);
                    _clientPlayer.Apply(UdpManager.CurData);
                    BlockGenerator.Instance.Apply(UdpManager.CurData);
                }
            }

            if (_isHost) {
                
                var curData = new GameData(
                    (_hostPlayer as CalculatePlayer)!.FSM,
                    (_clientPlayer as CalculatePlayer)!.FSM
                );
                
                UdpManager.Send(curData);
            }
            else 
                UdpManager.Send(input);
        }
    }
}