using System;
using Player;
using UnityEngine;

namespace Networking.InGame {
    public abstract class PlayerBase: MonoBehaviour {

        public bool IsHost { get; private set; } = false;
        
        public abstract void Apply(IData data);

        public void SetHost(bool isHost = true) =>
            IsHost = isHost;

        protected void Update() {
            if (!IsHost)
                return;
            
            CameraFocus.Update(gameObject);
        }
    }
    
    [RequireComponent(typeof(PlayerFSM))]
    [RequireComponent(typeof(Rigidbody))]   
    public class CalculatePlayer: PlayerBase {

        private PlayerFSM _fsm;
        private Rigidbody _rigid;
        private bool _isHost = false;

        public override void Apply(IData data) {
 
             if (data is not InputData inputData)
                 return;
             
             _fsm.SetInputData(inputData);
        }       
        
        private void Awake() {
            
            _fsm = GetComponent<PlayerFSM>();
            _rigid = GetComponent<Rigidbody>();
        }
    }

    
}