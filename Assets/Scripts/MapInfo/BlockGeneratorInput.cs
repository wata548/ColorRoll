using Networking.InGame;
using UnityEngine;

namespace MapInfo {
    public partial class BlockGenerator {
        public void Apply(GameData data) {
            foreach (var @break in data.BreakBlocks) {
                BreakBlock(@break);
            }

            for (int i = 0; i < _map.GetLength(0); i++) {
                for (int j = 0; j < _map.GetLength(1); j++) {
                    if (_map[i, j] == data.Map[i, j]) 
                        continue;
                    //generated - None
                    //Ungenerated - None
                    if (data.Map[i, j] == MapState.None) {
                        if(_blocks.TryGetValue(new(i, Height, j), out var target))
                            Destroy(target);

                    }
                    //Ungenerated - generated
                    else if (_map[i, j] == MapState.UnGenerateBlock) {
                        TryAdd(new(i, Height, j));
                    }

                    _map[i, j] = data.Map[i, j];
                }
            }
        }
    }
}