using Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Networking.RoomSystem {
    public class NetworkManager: MonoSingleton<NetworkManager> {

        public bool IsHost { get; private set; } = true;
        public RoomBase RoomBase { get; private set; }

        private new void Awake() {
            
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void EnterHostScene(string roomName) {
            IsHost = true;
            RoomBase = new RoomHost(roomName);
        }

        public void EnterClientScene() {
            IsHost = false;
            RoomBase = new RoomClient();
        }

        public void ExitRoom() {
            RoomBase = null;
        } 
    }
}