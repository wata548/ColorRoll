using TMPro;
using UnityEngine;

namespace UI.ClientRoom {
    public class RoomInfo: MonoBehaviour {

        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private TMP_Text _ip;

        public void Set(string name, string ip) {
            _roomName.text = name;
            _ip.text = ip;
        }
    }
}