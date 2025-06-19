using System;
using Extensions;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

namespace Test {
    public class FloorDrawer: MonoSingleton<FloorDrawer> {

       //==================================================||Concstant 
        private const int TextureDetail = 11;

       //==================================================||Properties 
        public float Player1Radius {
            get => _player1Radius;
            set {
                _player1Radius = value;
                _brush.SetFloat("_Radius1", _player1Radius);
            }
        }
        public float Player2Radius {
            get => _player2Radius;
            set {
                _player2Radius = value;
                _brush.SetFloat("_Radius2", _player2Radius);
            }
        }
        
       //==================================================||Fields 
        private float _player1Radius = 0.002f;
        private float _player2Radius = 0.002f;

        [SerializeField] private MeshRenderer _renderer;
        [SerializeField] private Material _brush;
        [SerializeField] private GameObject _target1; 
        [SerializeField] private GameObject _target2; 
        
        private RenderTexture _tex;
        private Vector2 _tempPos1 = Vector2.zero;
        private Vector2 _tempPos2 = Vector2.zero;

       //==================================================||Methods 
        private void Fill() {
            
            _brush.SetFloat("_Radius2", 0.002f);
            _brush.SetVector("_Pos1", _tempPos1);
            _brush.SetVector("_Pos2", _tempPos2);

            RenderTexture temp = RenderTexture.GetTemporary(_tex.width, _tex.height);
            Graphics.Blit(_tex, temp);
            Graphics.Blit(temp, _tex, _brush);

            RenderTexture.ReleaseTemporary(temp);
        }
        
       //==================================================||Unity 
        private new void Awake() {
            base.Awake();
            _tex = new RenderTexture(1 << TextureDetail, 1 << TextureDetail, 0, RenderTextureFormat.ARGB32);
            _tex.Create();
            _renderer.material.mainTexture = _tex;

            //Call property
            Player1Radius = Player1Radius;
            Player2Radius = Player2Radius;
        }

        private void Update() {

            bool needUpdate = false;
            if (Physics.Raycast(_target1.transform.position, Vector3.down, out var hit)) {
                if (hit.transform.CompareTag("Paintable")) {
                    needUpdate = true;
                    _tempPos1 = hit.textureCoord;
                }
            }
            if (Physics.Raycast(_target2.transform.position, Vector3.down, out hit)) {
                if (hit.transform.CompareTag("Paintable")) {
                    needUpdate = true;
                    _tempPos2 = hit.textureCoord;
                }
            }

            if (needUpdate)
                Fill();
        }
    }
}