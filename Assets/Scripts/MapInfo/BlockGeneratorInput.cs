using Networking.InGame;
using UnityEngine;

namespace MapInfo {
    public partial class BlockGenerator {
        public void Apply(string data) {
            var gameData = new GameData(data);
            foreach (var @break in gameData.BreakBlocks) {
                BreakBlock((Vector3)@break);
            }

            /*for (int i = 0; i < _map.GetLength(0); i++) {
                for (int j = 0; j < _map.GetLength(1); j++) {
                    if (_map[i, j] == gameData.Map[i, j]) 
                        continue;
                    //generated - None
                    //Ungenerated - None
                    if (gameData.Map[i, j] == MapState.None) {
                        if(_blocks.TryGetValue(new(i, Height, j), out var target))
                            Destroy(target);

                    }
                    //Ungenerated - generated
                    else if (_map[i, j] == MapState.UnGenerateBlock) {
                        TryAdd(new(i, Height, j));
                    }

                    _map[i, j] = gameData.Map[i, j];
                }
            }*/
        }
    }
}