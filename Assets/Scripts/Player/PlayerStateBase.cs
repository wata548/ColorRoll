using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using InputSystem;
using Networking;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player {
    public abstract class PlayerStateBase {

        protected static ReadOnlyDictionary<PlayerState, Type> StateType = new(
            new Dictionary<PlayerState, Type>() {
                { PlayerState.Working , typeof(WorkingState)},
                { PlayerState.Charging , typeof(ChargingState)},
            } 
        ); 
        
        protected Rigidbody _rigid;

        protected PlayerStateBase(Rigidbody rigid, PlayerState prev) {
            _rigid = rigid;
            Changed(prev);
        }
        
        protected abstract void Changed(PlayerState previousState);

        public abstract PlayerStateBase Update(InputData data, ref PlayerState state);
    }

    public class WorkingState: PlayerStateBase {

        public WorkingState(Rigidbody rigid, PlayerState prev)
            : base(rigid, prev) { }

        private static ReadOnlyDictionary<Actions, Vector2Int> DirectionKey = new(
            new Dictionary<Actions, Vector2Int>() {
                { Actions.Front, Vector2Int.up },
                { Actions.Back, Vector2Int.down },
                { Actions.Left, Vector2Int.left },
                { Actions.Right, Vector2Int.right },
            }
        );
        
        protected override void Changed(PlayerState previousState) {
            
        }
        
        public override PlayerStateBase Update(InputData data, ref PlayerState state ) {
            if ((data.GetKey & Actions.Charge) != Actions.None) {
                var temp = state;
                state = PlayerState.Charging;
                return new ChargingState(_rigid, state);
            }
                
            var result = Vector2.zero;
                
            foreach (var direction in DirectionKey) {
                if ((direction.Key & data.GetKey) != Actions.None)
                    result += direction.Value;
            }

            var view = Quaternion.Euler(0, -data.ViewDirection.eulerAngles.y, 0);
            _rigid.linearVelocity = (view * result).normalized;

            return this;
        }
    }

    public class ChargingState: PlayerStateBase {
        private PlayerState _prev;

        public ChargingState(Rigidbody rigid, PlayerState prev)
            : base(rigid, prev) {
            _prev = prev;
        }

        protected override void Changed(PlayerState previousState) {
        }

        public override PlayerStateBase Update(InputData data, ref PlayerState state) {
            
            if ((data.GetKey & Actions.Charge) == Actions.None) {
                var temp = state;
                state = _prev;
                return Activator.CreateInstance(StateType[_prev]) 
                    as PlayerStateBase;
            }

            return this;
        }
    }
}