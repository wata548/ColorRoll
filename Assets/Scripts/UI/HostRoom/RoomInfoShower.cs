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
            var room = (NetworkManager.Instance.Room as RoomHost);
            _roomName.text = string.Format(RoomNameFormat, room.RoomName);
            _myIp.text = RoomBase.GetIP();
        }

        private void Update() {
            var room = (NetworkManager.Instance.Room as RoomHost);
            _otherPlayerIp.text = room.OtherPlayerIp;
        }
    }
}