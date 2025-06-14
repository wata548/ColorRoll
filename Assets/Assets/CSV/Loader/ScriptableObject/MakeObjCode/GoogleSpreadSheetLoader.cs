#if UNITY_EDITOR

using System;
using System.Net.Http;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using CSVData.Extensions;

namespace CSVData {
    
    
    
    [CreateAssetMenu(menuName = "Loader/GoogleSpreadSheet")]
    public class GoogleSpreadSheetLoader: LoaderBase {
        
        
        [SerializeField] private string _apiKey;
        [Header(@"In spreadSheet(<size=70%>'https://docs.google.com/spreadsheets/d/XXX/edit?gid=YYY#gid=ZZZ'</size>) link, XXX is path")]
        [SerializeField] private string _path;
        [Header("RawData: sheet(page) name, TargetTypeName: type name(empty is recommanded)")]
        [SerializeField] private List<LoadInfo<string>> _infos;
        
        public override void Load(string directory) {

            HttpClient httpClient = new();
            foreach (var info in _infos) {

                if(!TempName(info, out var tempName))
                    continue;
                
                if(!info.SyncAble(tempName))
                    continue;

                var csvData = GetData(info, httpClient);
                SyncObject(info.TargetTypeName, csvData, directory);
            }
        }

        public override void Generate() {
            HttpClient httpClient = new();
            foreach (var info in _infos) {

                if (!TempName(info, out var tempName))
                    continue;
                
                if(!info.Generatable(tempName))
                    continue;

                bool needUpdate = !IsExistType(info.TargetTypeName);
                bool needUpdateTable = !IsExistType($"{info.TargetTypeName}Table");
                
                var csvData = GetData(info, httpClient);
                CSV.GenerateObjCode(info.TargetTypeName, csvData, info.csvDataStyle, needUpdateTable, needUpdate);
            }
        }

        public bool TempName(LoadInfo<string> info, out string tempName) {

            tempName = "";
            if (!string.IsNullOrWhiteSpace(info.TargetTypeName))
                return true;
            
            foreach (var candidate in info.RawDatas) {
                if(string.IsNullOrWhiteSpace(candidate)) 
                    continue;

                tempName = candidate.FixTypeName();
                
                break;
            }

            return !string.IsNullOrWhiteSpace(tempName);
        }
        
        public List<List<string>> GetData(LoadInfo<string> info, HttpClient httpClient) {
            string urlFormat =
                $"https://sheets.googleapis.com/v4/spreadsheets/{_path}/values/{{0}}?key={_apiKey}";

            bool isFirst = true;
            var csvData = new List<List<string>>();
            foreach (var sheet in info.RawDatas) {
                
                string url = string.Format(urlFormat, sheet);
             
                //get spread sheet data
                var spreadSheetData = "";
                try {

                    spreadSheetData = httpClient.GetStringAsync(url).Result;
                }
                catch (Exception){
                    Debug.Log($"{url} call generate bug");
                    throw;
                }    
                var temp = JsonConvert
                    .DeserializeObject<SpreadSheetType>(spreadSheetData).values;

                if (!isFirst) {
                    temp = temp
                        .Skip(2)
                        .ToList();
                }
                isFirst = false;

                csvData.AddRange(temp);
            }

            return csvData;
        }
        
        public class SpreadSheetType {

            public string range;
            public string majorDimension;
            public List<List<string>> values;
        }
    }
}

#endif