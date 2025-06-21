using Unity.VisualScripting;
using UnityEngine;

namespace Extensions {
    public class MonoSingleton<T>: MonoBehaviour where T: class {

        public static T Instance { get; private set; } = null;

        protected void Awake() {
            if (Instance.IsUnityNull()) {
                Instance = this as T;
            }
            else
                Destroy(this);
        }
        
    }
}