using System;
using System.Text;
using InputSystem;
using UnityEngine;

namespace Networking.InGame {
    public class InputData: IData {

        public Actions GetKey;
        public Actions GetKeyDown;
        public Actions GetKeyUp;
        public JsonQuaternion ViewDirection;

        public InputData(string data) {
            var temp = data.Split('\n');
            GetKey = (Actions)Convert.ToInt32(temp[0]);
            GetKeyDown = (Actions)Convert.ToInt32(temp[1]);
            GetKeyUp = (Actions)Convert.ToInt32(temp[2]);

            ViewDirection = new(temp[3]);
        }

        public override string ToString() {
            var builder = new StringBuilder();
            builder.AppendLine(((int)GetKey).ToString());
            builder.AppendLine(((int)GetKeyDown).ToString());
            builder.AppendLine(((int)GetKeyUp).ToString());
            builder.AppendLine(ViewDirection.ToString());

            return builder.ToString();
        }

        public InputData(Quaternion view, InputControl input) {
            
            ViewDirection   = new(view.normalized);
            GetKey          = input.GetKey;
            GetKeyDown      = input.GetKeyDown;
            GetKeyUp        = input.GetKeyUp;
        }
        
    }
}