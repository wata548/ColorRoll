using System.Collections.Generic;
using System.Collections.ObjectModel;
using InputSystem;
using Networking;
using UnityEngine;

namespace Player {
    public class WorkingStateBase: StateBase<PlayerState, PlayerFSM> {

       //==================================================||Constant 
        private static readonly ReadOnlyDictionary<Actions, Vector3Int> DirectionKey = new(
            new Dictionary<Actions, Vector3Int>() {
                { Actions.Front, Vector3Int.forward },
                { Actions.Back, Vector3Int.back },
                { Actions.Left, Vector3Int.left },
                { Actions.Right, Vector3Int.right },
            }
        );

       //==================================================||Constructors 
        public WorkingStateBase(PlayerState key) : base(key) {}
        
       //==================================================||Methods 
        public override void Enter(PlayerState previousState, PlayerFSM machine) {}
        public override void Exit(PlayerState nextState, PlayerFSM machine) {}

        public override void Update(PlayerFSM machine ) {
            if ((machine.Data.GetKey & Actions.Charge) != Actions.None) {
                machine.Change(PlayerState.Charging);
                return;
            }
                
            var result = Vector3.zero;
                
            foreach (var direction in DirectionKey) {
                if ((direction.Key & machine.Data.GetKey) != Actions.None)
                    result += direction.Value;
            }

            var view = Quaternion.Euler(0, machine.Data.ViewDirection.eulerAngles.y, 0);
            machine.Rigid.linearVelocity = (view * result).normalized;
        }
    }
}