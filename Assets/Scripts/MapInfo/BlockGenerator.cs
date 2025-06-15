using System;
using System.Collections.Generic;
using UnityEngine;

namespace MapInfo {
    public class BlockGenerator: MonoBehaviour {
        
        private const int DefaultMapScale = 4;
        private const int Thickness = 2;
        private const int MaxDeepth = 10;
        
        private Dictionary<Vector3, GameObject> _blocks = new();
        [SerializeField] private GameObject _prefab;
        
        private bool[,] _map = new bool[
            DefaultMapScale + MaxDeepth * 2, 
            DefaultMapScale + MaxDeepth * 2
        ];
        
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
                    var pos = new Vector3(startPos1 + i, 0, j);
                    if (!_blocks.ContainsKey(pos))
                        _blocks[pos] = Instantiate(_prefab, pos, Quaternion.identity);
                    pos = new Vector3(startPos2 + i, 0, j);
                    if (!_blocks.ContainsKey(pos))
                        _blocks[pos] = Instantiate(_prefab, pos, Quaternion.identity);
                    pos = new Vector3(j, 0, startPos1 + i);
                    if (!_blocks.ContainsKey(pos))
                        _blocks[pos] = Instantiate(_prefab, pos, Quaternion.identity);
                    pos = new Vector3(j, 0, startPos2 + i);
                    if (!_blocks.ContainsKey(pos))
                        _blocks[pos] = Instantiate(_prefab, pos, Quaternion.identity);
                }
            }
        }

        private void Awake() {
            SetUp();
        }
    }
}