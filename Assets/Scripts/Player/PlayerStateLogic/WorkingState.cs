using System.Collections.Generic;
using System.Collections.ObjectModel;
using InputSystem;
using Networking;
using UnityEngine;

namespace Player {
    public class WorkingState: PlayerStateBase {

        public WorkingState(Rigidbody rigid, PlayerState prev)
            : base(rigid, prev) { }

        private static ReadOnlyDictionary<Actions, Vector3Int> DirectionKey = new(
            new Dictionary<Actions, Vector3Int>() {
                { Actions.Front, Vector3Int.forward },
                { Actions.Back, Vector3Int.back },
                { Actions.Left, Vector3Int.left },
                { Actions.Right, Vector3Int.right },
            }
        );
        
        protected override void Enter(PlayerState previousState) {}
        protected override void Exit(PlayerState nextState) {}

        public override PlayerStateBase Update(InputData data, ref PlayerState state ) {
            if ((data.GetKey & Actions.Charge) != Actions.None) {
                var temp = state;
                state = PlayerState.Charging;
                return MakeByState(temp, state, _rigid);
            }
                
            var result = Vector3.zero;
                
            foreach (var direction in DirectionKey) {
                if ((direction.Key & data.GetKey) != Actions.None)
                    result += direction.Value;
            }

            var view = Quaternion.Euler(0, data.ViewDirection.eulerAngles.y, 0);
            _rigid.linearVelocity = (view * result).normalized;

            return this;
        }
    }
}