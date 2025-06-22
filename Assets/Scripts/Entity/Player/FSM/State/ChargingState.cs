using System.Collections.Generic;
using UnityEngine;
using InputSystem;
using FSMBase;

namespace Player {
    public class ChargingState: StateBase<PlayerState, PlayerFSM> {
        
        //==================================================||Constant
        private static readonly float[] LevelTime = new float[]{0, 1f, 2.8f};

        //==================================================||Fields
        private float _startTime = 0;
        
       //==================================================||Properties 
        public int ChargeLevel { get; private set; }= -1;

        //==================================================||Constructors 
        public ChargingState(PlayerState key) : base(key) {}
        
        //==================================================||Methods 

        public void InitLevel() => ChargeLevel = -1;
        
        public override void Enter(PlayerState previousState, PlayerFSM machine) {
            _startTime = Time.time;
            ChargeLevel = -1;
        }

        public override void Exit(PlayerState nextState, PlayerFSM machine) { }

        public override void Update(PlayerFSM machine) {
            if ((machine.Data.GetKey & Actions.Charge) == Actions.None) {
                
                foreach (var particle in machine.ChargeParticles) 
                    particle.Stop();
                machine.Change(PlayerState.Shooting);
                return;
            }

            foreach (var particle in machine.ChargeParticles) {
                particle.transform.localPosition = Vector3.zero;
                particle.transform.rotation = Quaternion.identity;
            }

            float progress = Time.time - _startTime;
            if (ChargeLevel + 1 < LevelTime.Length && progress >= LevelTime[ChargeLevel + 1]) {
                if (ChargeLevel >= 0 && ChargeLevel < LevelTime.Length) {
                    machine.ChargeParticles[ChargeLevel].Stop();
                }
                
                ChargeLevel++;
                if (ChargeLevel >= 0 && ChargeLevel < LevelTime.Length) {
                    machine.ChargeParticles[ChargeLevel].Play();
                }
            }
            
            machine.Rigid.linearVelocity = Vector3.zero;
        }
    }
}