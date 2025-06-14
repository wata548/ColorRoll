using System;
using System.Collections.Generic;
using CSVData;
using UnityEngine;

namespace Test {
    public class GenericTest: MonoBehaviour {

        private class Data {
            private int _a;
            private int _b;

            public Data(){}
            public Data(int a, int b) => (_a, _b) = (a, b);
        }
        
        
        private void Awake() {

            var data = new Dictionary<int, Data>();
            data.Add(1, new(2,3));
            data.Add(2, new(3,4));
            data.Add(3, new(4,5));
            data.Add(4, new(5,6));
            Debug.Log(CSV.Serialize(data));
            var newData = (CSV.DeserializeToList<Data>(CSV.Serialize(data)) as List<Data>)!;
            Debug.Log(newData);
        }
    }
}