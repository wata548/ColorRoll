using System.Collections.Generic;
using Networking;
using UnityEngine;

namespace Player {

    [RequireComponent(typeof(Rigidbody))]
    public class Player : MonoBehaviour {
        protected bool _isHost = false;
        protected Rigidbody _rigidbody;

        public void SetHost(bool isHost) =>
            _isHost = isHost;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }
    }
    
    public class SimplePlayer: Player{

        public void Apply(PlayerData data) {
            _rigidbody.linearVelocity = data.Velocity(_isHost);
            transform.localScale = data.Scale(_isHost);
            transform.position = data.Pos(_isHost);
        }
    }

    public class StatePlayer : Player {

        private PlayerState _curState;
        private Stack<PlayerState> _stateStack = new();
        
    }
}