using UnityEngine;
using InputSystem;
using Networking;

namespace Player {
    public class ChargingStateBase: StateBase<PlayerState, PlayerFSM> {
        private PlayerState _prev;

        public ChargingStateBase(PlayerState key) : base(key) {}
        
        public override void Enter(PlayerState previousState, PlayerFSM machine) {
            _prev = previousState;
        }

        public override void Exit(PlayerState nextState, PlayerFSM machine) { }

        public override void Update(PlayerFSM machine) {
            if ((machine.Data.GetKey & Actions.Charge) == Actions.None) {
                machine.Change(_prev);
            }
            machine.Rigid.linearVelocity = Vector3.zero;
        }
    }
}