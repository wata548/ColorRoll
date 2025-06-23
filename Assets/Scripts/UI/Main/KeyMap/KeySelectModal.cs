using System;
using InputSystem;
using TMPro;
using UnityEngine;

namespace UI.Main {
    public class KeySelectModal: MonoBehaviour {
        
        private Actions _action;
        private KeyCode _key;
        private KeyCode _initKey;
        [SerializeField] private GameObject _modal;
        [SerializeField] private TMP_Text _actionShower;
        [SerializeField] private TMP_Text _curKeyShower;
        private bool _isOpen = false;

        public bool IsOn => _isOpen;
        public Actions Action => _action;
        public KeyCode Key => _key;
        public KeyCode InitKey => _initKey;
        
        public void Open(Actions action, KeyCode cur) {

            if (_isOpen)
                return;
            
            _isOpen = true;
            
            _action = action;
            _initKey = cur;
            _key = cur;

            _modal.SetActive(true);
            _actionShower.text = _action.ToString();
            _curKeyShower.text = _key.ToString();
        }

        public void Close() {
            _isOpen = false;
            _modal.SetActive(false);
        }

        private KeyCode GetAnyKey() {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode))) {

                if (key == KeyCode.Mouse0)
                    continue;
                
                if (key != KeyCode.None && Input.GetKeyDown(key))
                    return key;
            }

            return KeyCode.None;
        }
        
        private void Update() {
            if (!_isOpen)
                return;

            var key = GetAnyKey();
            
            if (key != KeyCode.None) {
                
                _key = key;
                _curKeyShower.text = _key.ToString();
            }
        }
    }
}