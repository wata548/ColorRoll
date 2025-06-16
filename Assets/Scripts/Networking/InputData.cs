using InputSystem;
using UnityEngine;

namespace Networking {
    public class InputData {

        public readonly Actions GetKey;
        public readonly Actions GetKeyDown;
        public readonly Actions GetKeyUp;
        public readonly Quaternion ViewDirection;

        public InputData(Quaternion view, InputControl input) {
            
            ViewDirection   = view.normalized;
            GetKey          = input.GetKey;
            GetKeyDown      = input.GetKeyDown;
            GetKeyUp        = input.GetKeyUp;
        }
        
    }
}