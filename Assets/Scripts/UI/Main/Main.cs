using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main {
    public class Main: MonoBehaviour {

        [SerializeField] private MakeRoomModal _modal;
        
        public void Quit() {
            Application.Quit();
        }

        public void Join() {
            SceneManager.LoadScene("JoinRoom");
        }

        public void Make() {
            _modal.SetActive(true);
            InteractableUIBase.SetGroup("MainButtons", false);
        }

        public void CloseMakeRoomModal() {
            InteractableUIBase.SetGroup("MainButtons", true);
            _modal.SetActive(false);
            _modal.Init();
        }
    }
}