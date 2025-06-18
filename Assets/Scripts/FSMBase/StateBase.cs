namespace FSMBase {
    public abstract class StateBase<TState, TMachine> {

       //==================================================||Properties 
        public readonly TState Key;

       //==================================================||Constructors 
        public StateBase(TState key) =>
            Key = key;
        
       //==================================================||Methods 
        public abstract void Enter(TState previousState, TMachine machine);
        public abstract void Exit(TState nextState, TMachine machine);
        public abstract void Update(TMachine machine);
    }
}