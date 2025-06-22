using System;
using Networking.RoomSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.HostRoom {
    public class Main: MonoBehaviour {

        public void MainScence() {
            (NetworkManager.Instance.Room as RoomHost)!.Quit();
            SceneManager.LoadScene("Main");
        }

        public void Start() {
            
            (NetworkManager.Instance.Room as RoomHost)!.Start();
        }
    }
}