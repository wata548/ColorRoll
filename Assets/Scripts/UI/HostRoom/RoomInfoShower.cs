using System;
using Networking.RoomSystem;
using TMPro;
using UnityEngine;

namespace UI.HostRoom {
    public class RoomInfoShower: MonoBehaviour {

        private const string RoomNameFormat = "Room Name: {0}";
        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private TMP_Text _myIp;
        [SerializeField] private TMP_Text _otherPlayerIp;

        private void Awake() {
            _roomName.text = string.Format(RoomNameFormat, RoomHost.RoomName);
            _myIp.text = RoomBase.GetIP();
        }

        private void Update() {
            _otherPlayerIp.text = RoomHost.OtherPlayerIp;
        }
    }
}