using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Networking.RoomSystem;
using UnityEngine;

namespace UI.ClientRoom {
    public class RoomInfoButton: MonoSingleton<RoomInfoButton> {

        private static readonly Vector2 DefaultPos = new(-460, 420);
        private const float YInterval = 160;
        
        [SerializeField] private RoomInfo _prefab;
        private List<RoomInfo> _rooms = new();
        private int _count = 0;

        private void UpdateRoomInfo(List<(string roomName, string ip)> datas) {
            foreach (var room in _rooms) {
                Destroy(room);
            }

            int idx = 0;
            foreach (var data in datas) {
                var target = Instantiate(_prefab.gameObject);

                var pos = DefaultPos;
                pos.y -= YInterval * idx;
                target.transform.position = pos;
                target.GetComponent<RoomInfo>().Set(data.roomName, data.ip);

                idx++;

            }
            Debug.Log($"Updated (count: {datas.Count})");
        }

        public void Refresh() {
            
            var target = (NetworkManager.Instance.RoomBase as RoomClient)!;
            target.Refresh();

            StartCoroutine(
                ExCoroutine.WaitStart(
                    RoomClient.WaitReceiveTime,
                    () => UpdateRoomInfo(target.FindedRoom)
                )
            );
            
        }

        private new void Awake() {
            base.Awake();
            Refresh();
        }
    }
}