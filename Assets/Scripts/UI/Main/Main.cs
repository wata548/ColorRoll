using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.Main {
    public class Main: MonoBehaviour {

        [SerializeField] private MakeRoomModal _modal;
        public const string ButtonGroup = "MainButtons";
        
        public void Quit() {
            Application.Quit();
        }

        public void Join() {
            SceneManager.LoadScene("JoinRoom");
        }

        public void Make() {
            _modal.SetActive(true);
            InteractableUIBase.SetGroup(ButtonGroup, false);
        }

        public void CloseMakeRoomModal() {
            InteractableUIBase.SetGroup(ButtonGroup, true);
            _modal.SetActive(false);
            _modal.Init();
        }
    }
}