using System;
using System.Collections.Generic;
using Extensions;
using UnityEngine;

namespace MapInfo {
    public class BlockGenerator: MonoSingleton<BlockGenerator> {
        
        private const int DefaultMapScale = 4;
        private const int Thickness = 2;
        private const int MaxDeepth = 10;
        private const float Height = 0.5f;
        
        private Dictionary<Vector3, GameObject> _blocks = new();
        [SerializeField] private GameObject _prefab;
        
        //t => empty
        //f => fill
        private bool[,] _map = new bool[
            DefaultMapScale + MaxDeepth * 2, 
            DefaultMapScale + MaxDeepth * 2
        ];

        public void DeletedBlock(Vector3 pos) {
            _map[(int)pos.x, (int)pos.z] = true;
            
            for (int i = (int)pos.x - Thickness; i <= (int)pos.x + Thickness; i++) {
                for (int j = (int)pos.z - Thickness; j <= (int)pos.z + Thickness; j++) {

                    TryAdd(new(i, Height, j));
                }
            }
        }
        
        private void SetUp() {

            int startPos1 = MaxDeepth - Thickness;
            int startPos2 = MaxDeepth + DefaultMapScale;

            for (int i = 0; i < DefaultMapScale; i++) {
                for (int j = 0; j < DefaultMapScale; j++) {
                    _map[i + MaxDeepth, j + MaxDeepth] = true;
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
            
            if (!_map[(int)pos.x, (int)pos.z] && !_blocks.ContainsKey(pos))
                _blocks[pos] = Instantiate(_prefab, pos, Quaternion.identity);
        }

        private new void Awake() {
            base.Awake();
            SetUp();
        }
    }
}