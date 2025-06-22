using System;
using Player;
using UnityEngine;

namespace Networking.InGame {
    public abstract class PlayerBase: MonoBehaviour {

        public bool IsHost { get; private set; } = false;
        
        public abstract void Apply(string data);

        public void SetHost(bool isHost = true) =>
            IsHost = isHost;

        
    }
    
    [RequireComponent(typeof(PlayerFSM))]
    [RequireComponent(typeof(Rigidbody))]   
    public class CalculatePlayer: PlayerBase {

        private PlayerFSM _fsm;
        private Rigidbody _rigid;
        private bool _isHost = false;

        public PlayerFSM FSM => _fsm;

        public override void Apply(string data) {
             
             _fsm.SetInputData(new InputData(data));
        }       
        
        private void Awake() {
            
            _fsm = GetComponent<PlayerFSM>();
            _rigid = GetComponent<Rigidbody>();
            _fsm.Change(PlayerState.Working);
        }
        
        private void Update() {

            if (!IsHost)
                return;
            
            CameraFocus.Update(gameObject);
        }
    }
}