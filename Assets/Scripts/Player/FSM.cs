
using Networking;

namespace Player {
    public class FSM<STATE, MACHINE> {

        private StateBase<STATE, MACHINE> _curLogic;
        private STATE _curState = default(STATE);

        public void Change(StateBase<STATE, MACHINE> newLogic,STATE newState, MACHINE machine) {
            
            _curLogic?.Exit(newState, machine);
            _curLogic = newLogic;
            _curLogic?.Enter(_curState, machine);
            
            _curState = newState;
        }

        public void Update(MACHINE machine) {
            _curLogic?.Update(machine);
        }
    }
}