using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using CSVData;
using Extensions;
using UnityEngine;

namespace InputSystem {
    public class InputSystem {

        //==================================================||Constance
        private const string KeyBindFolder = "KeyBind";
        
        //==================================================||Fields 
        private Dictionary<Actions, KeyBindRow> _keyBind = new();

        //==================================================||Properties 
        public KeyCode this[Actions action] =>
            ExEnum.IsFlag(action) ? _keyBind[action].Key : KeyCode.None;

        public Actions GetKey =>
            CurrentKey(Input.GetKey);

        public Actions GetKeyDown =>
            CurrentKey(Input.GetKeyDown);

        public Actions GetKeyUp =>
            CurrentKey(Input.GetKeyUp);
       
        //==================================================||Methods

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
            var tempSetting = CSV.DeserializeToDictionary<KeyBindRow>(rawCsvData, "Key") 
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