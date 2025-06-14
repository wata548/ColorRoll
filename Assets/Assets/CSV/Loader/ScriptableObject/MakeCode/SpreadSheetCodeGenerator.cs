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

    [CreateAssetMenu(menuName = "Loader/Generator/SpreadSheet")]
    public class SpreadSheetCodeGenerator : CodeGenerateLoaderBase {
        
        [SerializeField] private string _apiKey;
        [Header(@"In spreadSheet(<size=70%>'https://docs.google.com/spreadsheets/d/XXX/edit?gid=YYY#gid=ZZZ'</size>) link, XXX is path")]
        [SerializeField] private string _path;
        [Header("Key: sheet(page) name, Value: type name(empty is recommanded)")]
        [SerializeField] private List<SerializablePair<string, string>> _infos;
        
        public override void Load(string directory) {}

        public override void Generate() {
            
            string urlFormat =
                $"https://sheets.googleapis.com/v4/spreadsheets/{_path}/values/{{0}}?key={_apiKey}";

            var httpClient = new HttpClient();
            
            foreach (var sheet in _infos) {
                
                if(string.IsNullOrWhiteSpace(sheet.Key))
                    continue;

                if (string.IsNullOrWhiteSpace(sheet.Value))
                    sheet.Value = sheet.Key;

                sheet.Value = sheet.Value.FixTypeName();
                    
                string url = string.Format(urlFormat, sheet.Key);
             
                //get spread sheet data
                var spreadSheetData = "";
                try {

                    spreadSheetData = httpClient.GetStringAsync(url).Result;
                }
                catch (Exception){
                    Debug.Log($"{url} call generate bug");
                    throw;
                }    
                var csvData = JsonConvert
                    .DeserializeObject<SpreadSheetType>(spreadSheetData).values;

                CSV.GenerateCode(sheet.Value,csvData);
            }
        }
    }
}

#endif