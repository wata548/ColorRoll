using System;
using Extensions;
using UnityEngine;

namespace Networking.RoomSystem {
    public class NetworkManager: MonoSingleton<NetworkManager> {

       //==================================================||Properties 
        [field: SerializeField]public bool IsHost { get; private set; } = true;
        public RoomBase Room { get; private set; }

       //==================================================||Unity 
        private new void Awake() {
            
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationQuit() {
            if (Room == null)
                return;
            if (Room is RoomHost host) {
                
                host.Quit();
            }
            else if (Room is RoomClient client) {
                
                client.Quit();
                client.Close();
            }
        }

        //==================================================||Methods 
        public void EnterHostScene(string roomName) {
            
            IsHost = true;
            Room = new RoomHost(roomName);
        }

        public void EnterClientScene() {
            
            IsHost = false;
            Room = new RoomClient();
        }

        public void ExitRoom() {
            Room = null;
        } 
    }
}