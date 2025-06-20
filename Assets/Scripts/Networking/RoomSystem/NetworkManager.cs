using Extensions;
using UnityEngine;
using UnityEngine.Serialization;

namespace Networking.RoomSystem {
    public class NetworkManager: MonoSingleton<NetworkManager> {

       //==================================================||Properties 
        public bool IsHost { get; private set; } = true;
        public RoomBase RoomBase { get; private set; }

       //==================================================||Unity 
        private new void Awake() {
            
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

       //==================================================||Methods 
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