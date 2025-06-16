using UnityEngine;
using InputSystem;
using Networking;

namespace Player {
    public class ChargingState: PlayerStateBase {
        private PlayerState _prev;

        public ChargingState(Rigidbody rigid, PlayerState prev)
            : base(rigid, prev) {
            _prev = prev;
        }

        protected override void Enter(PlayerState previousState) {
            _rigid.linearVelocity = Vector3.zero;
        }

        protected override void Exit(PlayerState nextState) { }

        public override PlayerStateBase Update(InputData data, ref PlayerState state) {
            
            if ((data.GetKey & Actions.Charge) == Actions.None) {
                var temp = state;
                state = _prev;
                return MakeByState(temp, state, _rigid);
            }

            return this;
        }
    }
}