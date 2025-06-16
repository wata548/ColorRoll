using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CSVData;
using Extensions;
using UnityEngine;

namespace InputSystem {
    public class InputControl {

        //==================================================||Constance
        private const string KeyBindFolder = "KeyBind";
        
        //==================================================||Fields 
        private Dictionary<Actions, KeyBindRow> _keyBind;
        private int _lastUpdateFrame = -1;
        
        private Actions _key;
        private Actions _keyDown;
        private Actions _keyUp;

        //==================================================||Properties 
        public KeyCode this[Actions action] =>
            ExEnum.IsFlag(action) ? _keyBind[action].Key : KeyCode.None;

        public Actions GetKey {
            get {
                Update();
                return _key;
            }
        }

        public Actions GetKeyDown {
            get {
                Update();
                return _keyDown;
            }
        }

        public Actions GetKeyUp {
            get {
                Update();
                return _keyUp;
            }
        }
       
        //==================================================||Constructor
        public InputControl(string path) =>
            SetKeyBind(path);
        public InputControl(TextAsset asset) =>
            SetKeyBind(asset);
        
        //==================================================||Methods

        public void Update() {
            if (_lastUpdateFrame == Time.frameCount)
                return;

            _lastUpdateFrame = Time.frameCount;
            _key        = CurrentKey(Input.GetKey);
            _keyUp      = CurrentKey(Input.GetKeyUp);
            _keyDown    = CurrentKey(Input.GetKeyDown);
        }
        
        public Actions CurrentKey(Func<KeyCode, bool> condition) {
            
            var result = Actions.None;
            foreach (var key in _keyBind) {
                if (condition(key.Value)) {
                    result |= key.Key;
                }
            }
            
            return result;
        }
        
        public void SetKeyBind(TextAsset asset) {
            var rawCsvData = asset.text;
            _keyBind = CSV.DeserializeToDictionary<KeyBindRow>(rawCsvData, "Key") 
                as Dictionary<Actions, KeyBindRow>;
        }

        /// <summary>
        /// in streaming assets
        /// </summary>
        /// <param name="path">translated to StreamingAssets/KeyBind/{path}</param>
        public void SetKeyBind(string path) {

            var keyBindFolderPath = Path.Combine(Application.streamingAssetsPath, KeyBindFolder);
            var keyBindFolder = new DirectoryInfo(KeyBindFolder);
            if(!keyBindFolder.Exists)
                keyBindFolder.Create();
            SetKeyBindByPath(Path.Combine(keyBindFolderPath, path));
        }
        
        public void SetKeyBindByPath(string path) {

            if (!(new FileInfo(path)).Exists)
                return;
            
            var rawCsvData = File.ReadAllText(path);
            _keyBind = CSV.DeserializeToDictionary<KeyBindRow>(rawCsvData, "Action") 
                as Dictionary<Actions, KeyBindRow>;
        }

        public void SaveKeyBind(string path) {
            path = Path.Combine(Application.streamingAssetsPath, KeyBindFolder);
            File.WriteAllText(path, CSV.Serialize(_keyBind));
        }
        
        private class KeyBindRow {
            public Actions Action { get; private set; }
            public KeyCode Key { get; private set; }
            
            public static implicit operator KeyCode(KeyBindRow target) =>
                target.Key;
        }
    }
}