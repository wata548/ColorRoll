using UnityEngine;

namespace Networking.RoomSystem {
    public class NetworkManager: MonoBehaviour {

        [SerializeField] private bool _isHost = true;
        private RoomBase _roomBase;

        private void Awake() {
            if (_isHost)
                _roomBase = new RoomHost();
            else
                _roomBase = new RoomClient();
        }
    }
}