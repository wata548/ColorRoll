using System;
using Networking.RoomSystem;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI.ClientRoom {
    public class RoomModal: MonoBehaviour {

        
        private const string RoomNameFormat = "Room Name: {0}";
        
        [SerializeField] private GameObject _modal;
        [SerializeField] private RoomInfoBuottonGenerator _generator;

        [Space] 
        [Header("Modal")] 
        [SerializeField] private TMP_Text _roomName;
        [SerializeField] private TMP_Text _myIp;
        [SerializeField] private TMP_Text _otherIp;

        private bool _isActive = false;
        private RoomClient _target = null;
        
        public void MainScene() {
                    
            _target = (NetworkManager.Instance.Room as RoomClient)!;
            _target.Close();
            SceneManager.LoadScene("Main");
        }
                
        
        public void QuitRoom() {
            
            _target = (NetworkManager.Instance.Room as RoomClient)!;
            _target.Quit();
            _modal.SetActive(false);
            
            _generator.enabled = true;
            _generator.Refresh();
            
            _isActive = false;
        }

        private void OpenModal() {
            _modal.SetActive(true);
            
            _generator.TurnOff();

            _target = (NetworkManager.Instance.Room as RoomClient)!;
            _myIp.text = RoomBase.GetIP();
            _roomName.text = string.Format(RoomNameFormat, _target.RoomName);
            _otherIp.text = _target.OtherPlayerIp;
            
            _isActive = true;
        }

        private void Update() {

            if (_isActive)
                return;
            
            _target = (NetworkManager.Instance.Room as RoomClient)!;
            
            if(!string.IsNullOrWhiteSpace(_target.OtherPlayerIp))
                OpenModal();
        }
    }
}