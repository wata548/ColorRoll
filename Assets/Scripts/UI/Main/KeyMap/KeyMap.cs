using System;
using System.Collections.Generic;
using System.Linq;
using CSVData;
using Extensions;
using InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Main {
    
    public class KeyMap: MonoBehaviour {

        [SerializeField] private GameObject _showModal;
        [SerializeField] private KeyMapButton[] _buttons;
        [SerializeField] private KeySelectModal _selectModal;
        private Dictionary<KeyCode , Actions> _cur = new();

        private const string DefaultPath = "DefaultInput.csv";
        private const string ButtonGroup = "MainButtons";

        private bool _isOpen = false;
        
        public void OpenModal() {

            _isOpen = true;
            InteractableUIBase.SetGroup(ButtonGroup, false);
            var cur = new InputControl(DefaultPath);
            _showModal.SetActive(true);

            int idx = 0;
            _cur.Clear();
            foreach (Actions action in Enum.GetValues(typeof(Actions))) {
                
                if(!ExEnum.IsFlag(action))
                    continue;
                
                _buttons[idx++].Set(action, cur[action]);
                _cur[cur[action]] = action;
            }
        }

        public void Select() {
            if (_cur.ContainsKey(_selectModal.Key)) {
                (_cur[_selectModal.InitKey], _cur[_selectModal.Key]) =
                    (_cur[_selectModal.Key], _cur[_selectModal.InitKey]);
            }
            else {
                _cur.Remove(_selectModal.InitKey);
                _cur.Add(_selectModal.Key, _selectModal.Action);
            }

            int idx = 0;
            var data = _cur.OrderBy(element => element.Value);
            foreach (var key in data) {
                
                _buttons[idx++].Set(key.Value, key.Key);
            }

            _selectModal.Close();
        }
        
        public void Save() {
            if (_selectModal.IsOn)
                return;
            
            var data = new Dictionary<Actions, KeyCode>();
            foreach (var button in _buttons) {
                data.Add(button.Data.Key, button.Data.Value);
            }

            InputControl.Save(data, DefaultPath);
            CloseModal();
        }

        public void CloseModal() {
            _isOpen = false;
            InteractableUIBase.SetGroup(ButtonGroup, true);
            _showModal.SetActive(false);
        }
        
        private void Update() {

            if (!_isOpen)
                return;
            var data = KeyMapButton.ClickedAction;
            if (data.Item1 != Actions.None) {
                _selectModal.Open(data.Item1, data.Item2);
            }
        }
    }
}