namespace Player {
    public class FiniteStateMachine<TState, TMachine> {

        //==================================================||Fields 
        private StateBase<TState, TMachine> _curLogic;
        private TState _curState = default(TState);

       //==================================================||Properties 
        public TState CurrentState => _curState;

       //==================================================||Methods 
        public void Change(StateBase<TState, TMachine> newLogic,TState newState, TMachine machine) {
            
            _curLogic?.Exit(newState, machine);
            _curLogic = newLogic;
            _curLogic?.Enter(_curState, machine);
            
            _curState = newState;
        }

        public void Update(TMachine machine) {
            _curLogic?.Update(machine);
        }
    }
}