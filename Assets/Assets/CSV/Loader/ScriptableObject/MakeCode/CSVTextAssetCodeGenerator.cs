#if UNITY_EDITOR

using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Net.Http;
using CSVData.Extensions;
using Newtonsoft.Json;
using SpreadSheetType = CSVData.GoogleSpreadSheetLoader.SpreadSheetType;

namespace CSVData {

    [CreateAssetMenu(menuName = "Loader/Generator/File")]
    public class CSVTextAssetCodeGenerator : CodeGenerateLoaderBase {
        
        [Header("Key: CSV File, Value: type name(empty is recommanded)")]
        [SerializeField] private List<SerializablePair<TextAsset, string>> _infos;
        
        public override void Load(string directory) {}

        public override void Generate() {
            
            foreach (var sheet in _infos) {
                
                if(sheet.Key == null)
                    continue;

                if (string.IsNullOrWhiteSpace(sheet.Value))
                    sheet.Value = sheet.Key.name;

                sheet.Value = sheet.Value.FixTypeName();

                var csvData = CSV.Parse(sheet.Key.text);
                CSV.GenerateCode(sheet.Value, csvData);
            }
        }
    }
}

#endif