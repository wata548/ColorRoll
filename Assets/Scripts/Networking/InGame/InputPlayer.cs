using Player;
using RayFire;
using UnityEngine;

namespace Networking.InGame {
    
    public class InputPlayer : PlayerBase {

        private Rigidbody _rigid;
        [SerializeField] private ParticleSystem[] _chargeParticles;
        
        [SerializeField] private RayfireRigid _breakPrefab;
        [SerializeField] private Collider _collider;
        [SerializeField] private MeshRenderer _renderer;

        public void Break() {
            _rigid.useGravity = false;
            _collider.isTrigger = true;
                                
            _renderer.enabled = false;
            Instantiate(_breakPrefab, transform.position, Quaternion.identity)
                .GetComponent<RayfireRigid>()
                .Demolish();
        }
        
        public void Restore() {
            _rigid.useGravity = true;
            _collider.isTrigger = false;
            _renderer.enabled = true;
        }
        
        private bool _isBreak = false;
        
        public override void Apply(IData data) {

            if (data is not GameData gameData)
                return;

            var velocity = IsHost ? gameData.HostVelocity : gameData.ClientVelocity;
            var position = IsHost ? gameData.HostPosition : gameData.ClientPosition;
            var particle = IsHost ? gameData.HostChargeLevel : gameData.ClientChargeLevel;
            var isBreak = _isBreak ? gameData.HostBreak : gameData.ClientBreak; 
            
            _rigid.linearVelocity = (Vector3)velocity;
            transform.position = (Vector3)position;

            //break process
            if (isBreak ^ _isBreak) {
                _isBreak = isBreak;
                if(_isBreak)
                    Break();
                else 
                    Restore();
            }
            
            //chargeParticle
            for (int i = 0; i < _chargeParticles.Length; i++) {
                if (particle == i) {
                    if(!_chargeParticles[particle].isPlaying)
                        _chargeParticles[particle].Play();
                }
                else
                    _chargeParticles[particle].Stop();
            }
        }
        
        private void Update() {

            if (IsHost)
                return;
            
            CameraFocus.Update(gameObject);
        }
    }    
}