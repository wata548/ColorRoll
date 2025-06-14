using System.IO;
using System.Collections.Generic;
using CSVData;
using Extensions;
using UnityEngine;

namespace InputSystem {
    public class InputSystem {

        //==================================================||Fields 
        private Dictionary<Actions, KeyBindRow> _keyBind = new();

        //==================================================||Properties 
        public KeyCode this[Actions action] =>
            EnumExtension.IsFlag(action) ? _keyBind[action].Value : KeyCode.None;
        
        //==================================================||Methods

        public void SetKeyBind(TextAsset asset) {
            var rawCsvData = asset.text;
            _keyBind = (Dictionary<Actions, KeyBindRow>) 
                CSV.DeserializeToDictionary<KeyBindRow>(rawCsvData, "Key");
        }
        
        public void SetKeyBind(string path) {
            
            var rawCsvData = File.ReadAllText(path);
            _keyBind = (Dictionary<Actions, KeyBindRow>) 
                CSV.DeserializeToDictionary<KeyBindRow>(rawCsvData, "Key");
        }

        private class KeyBindRow {
            public Actions Key { get; private set; }
            public KeyCode Value { get; private set; }
        }
    }
}