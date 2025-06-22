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
                
        
        public void CloseModal() {
            
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

            _myIp.text = RoomBase.GetIP();
            _roomName.text = string.Format(RoomNameFormat,  RoomClient.RoomName);
            _otherIp.text = RoomClient.OtherPlayerIp;
            
            _isActive = true;
        }

        private void Update() {

            RoomClient.Start();
            if (_isActive) {
                if (!RoomClient.IsInRoom) {
                    _modal.SetActive(false);
            
                    _generator.enabled = true;
                    _generator.Refresh();
            
                    _isActive = false;
                }    
                return;
            }
            
            if(!string.IsNullOrWhiteSpace(RoomClient.OtherPlayerIp))
                OpenModal();
            
            
        }
    }
}