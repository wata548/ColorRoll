
using System.Collections.Generic;
using Extensions;
using InputSystem;
using TMPro;
using UnityEngine;

namespace UI.Main {
    public class KeyMapButton : MonoBehaviour {

        public KeyValuePair<Actions, KeyCode> Data => new(_action, _key);
        
        private static (Actions, KeyCode) _clickedActions = (Actions.None, KeyCode.None);

        public static (Actions, KeyCode) ClickedAction {
            get {
                var temp = _clickedActions;
                _clickedActions = (Actions.None, KeyCode.None);
                return temp;
            }
            set {
                if (_clickedActions.Item1 == Actions.None)
                    _clickedActions = value;
            }
        }
        
        [SerializeField] private TMP_Text _context;
        private Actions _action;
        private KeyCode _key;
        
        public void Set(Actions actions, KeyCode code) {
            if (ExEnum.IsFlag(actions)) {
                
                _action = actions;
                _key = code;
                _context.text = $"{_action}: {code}";
            }
        }

        public void Click() => ClickedAction = (_action, _key);
    }
}