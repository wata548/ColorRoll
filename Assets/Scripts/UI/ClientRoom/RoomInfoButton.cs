using Networking.RoomSystem;
using TMPro;
using UnityEngine;

namespace UI.ClientRoom {
    public class RoomInfoButton: MonoBehaviour {

        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private TMP_Text _ip;

        public void Set(string name, string ip) {
            _roomName.text = name;
            _ip.text = ip;
        }

        public void Click() {
            var target = (NetworkManager.Instance.Room as RoomClient)!;
            target.RoomJoinRequest(_roomName.text);
        }
    }
}