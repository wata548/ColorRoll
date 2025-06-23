using System;
using Networking.InGame;
using Networking.RoomSystem;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main {
    public class Main: MonoBehaviour {

        [SerializeField] private MakeRoomModal _modal;
        public const string ButtonGroup = "MainButtons";

        private void Awake() {
            Application.runInBackground = true;
        }

        public void TutorialScene() =>
            SceneManager.LoadScene("TutorialScence");
        
        public void Quit() {
            Application.Quit();
        }

        public void Join() {
            
            NetworkManager.Instance.EnterClientScene();
            SceneManager.LoadScene("JoinRoom");
        }

        public void ShowMakeRoomModal() {
            _modal.SetActive(true);
            InteractableUIBase.SetGroup(ButtonGroup, false);
        }

        public void MakeRoom() {
            
            NetworkManager.Instance.EnterHostScene(_modal.Context);
            SceneManager.LoadScene("MakeRoom");
        }

        public void CloseMakeRoomModal() {
            InteractableUIBase.SetGroup(ButtonGroup, true);
            _modal.SetActive(false);
            _modal.Init();
        }
    }
}