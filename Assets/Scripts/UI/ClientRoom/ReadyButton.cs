using System;
using Networking.RoomSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.ClientRoom {
    public class ReadyButton: MonoBehaviour {

        private const string ReadyMessage = "Press to Ready";
        private const string CancelMessage = "Press to Cancel";
        
        [Header("UI")]
        [SerializeField] private TMP_Text _context;
        [SerializeField] private Image _button;
        [Space]
        [Header("Ready")]
        [SerializeField] private Color _readyButtonColor;
        [SerializeField] private Color _readyTextColor;
        [Space]
        [Header("Cancel")]
        [SerializeField] private Color _cancelButtonColor;
        [SerializeField] private Color _cancelTextColor;

        private bool _isOn = false;
        
        public void Click() {
            _isOn = !_isOn;
            ApplyColor();
            (NetworkManager.Instance.Room as RoomClient)!.Ready();
        }

        private void ApplyColor() {
            
            if (_isOn) {
                _context.color = _cancelTextColor;
                _button.color = _cancelButtonColor;
                _context.text = CancelMessage;
            }
            else {
                _context.color = _readyTextColor;
                _button.color = _readyButtonColor;
                _context.text = ReadyMessage;
            }
        }

        private void Awake() {
            ApplyColor();
        }
    }
}