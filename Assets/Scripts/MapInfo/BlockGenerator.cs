using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace MapInfo {
    public partial class BlockGenerator: MonoSingleton<BlockGenerator> {
        
        //==================================================||Constant        
        private const int DefaultMapScale = 4;
        private const int Thickness = 2;
        private const int MaxDeepth = 10;
        private const float Height = 0.5f;

        //==================================================||Properties        
        public List<Vector3> Break {
            get {
                var temp = _break;
                _break = new();
                return temp;
            }
        }
        public MapState[,] Map => _map;

        public int BreakCount { get; private set; } = 0;
       //==================================================||Fields 
        private List<Vector3> _break = new();

        private Dictionary<Vector3, GameObject> _blocks = new();
        [SerializeField] private GameObject _prefab;
        
        //t => empty
        //f => fill
        private MapState[,] _map = new MapState[
            DefaultMapScale + MaxDeepth * 2, 
            DefaultMapScale + MaxDeepth * 2
        ];

        public void BreakBlock(Vector3 pos) {

            BreakCount++;
            
            _break.Add(pos);
            
            _map[(int)pos.x, (int)pos.z] = MapState.None;
            
            for (int i = (int)pos.x - Thickness; i <= (int)pos.x + Thickness; i++) {
                for (int j = (int)pos.z - Thickness; j <= (int)pos.z + Thickness; j++) {

                    TryAdd(new(i, Height, j));
                }
            }
        }
        
        public void SetUp() {

            int startPos1 = MaxDeepth - Thickness;
            int startPos2 = MaxDeepth + DefaultMapScale;

            for (int i = 0; i < DefaultMapScale; i++) {
                for (int j = 0; j < DefaultMapScale; j++) {
                    _map[i + MaxDeepth, j + MaxDeepth] = MapState.None;
                }
            }
            
            for (int i = 0; i < Thickness; i++) {
                for (int j = startPos1; j < startPos2 + Thickness; j++) {
                    var pos = new Vector3(startPos1 + i, Height, j);
                    TryAdd(pos);
                    
                    pos = new Vector3(startPos2 + i, Height, j);
                    TryAdd(pos);
                    
                    pos = new Vector3(j, Height, startPos1 + i);
                    TryAdd(pos);
                    
                    pos = new Vector3(j, Height, startPos2 + i);
                    TryAdd(pos);

                }
            }
        }
        private void TryAdd(Vector3 pos) {
            if (pos.x >= _map.GetLength(0) || pos.z >= _map.GetLength(1))
                return;

            if (_map[(int)pos.x, (int)pos.z] == MapState.UnGenerateBlock) {
                
                _map[(int)pos.x, (int)pos.z] = MapState.GenerateBlock;
                _blocks[pos] = Instantiate(_prefab, pos, Quaternion.identity);
            }
        }

        private new void Awake() {
            base.Awake();
        }
    }
}