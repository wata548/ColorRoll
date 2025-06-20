using System;
using System.Collections;
using System.Collections.Generic;
using Extensions;
using Networking.RoomSystem;
using UnityEngine;

namespace UI.ClientRoom {
    public class RoomInfoBuottonGenerator: MonoSingleton<RoomInfoBuottonGenerator> {

        private static readonly Vector2 DefaultPos = new(500, 960);
        private const float YInterval = 160;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private RoomInfoButton _prefab;
        private List<RoomInfoButton> _rooms = new();
        private int _count = 0;

        private void UpdateRoomInfo(List<(string roomName, string ip)> datas) {
            foreach (var room in _rooms) {
                Destroy(room);
            }

            int idx = 0;
            foreach (var data in datas) {
                var target = Instantiate(_prefab.gameObject, _canvas.transform);

                var pos = DefaultPos;
                pos.y -= YInterval * idx;
                target.transform.position = pos;
                target.GetComponent<RoomInfoButton>().Set(data.roomName, data.ip);

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
                    () => UpdateRoomInfo(target.FindRoom)
                )
            );
            
        }

        private new void Awake() {
            base.Awake();
            Refresh();
        }
    }
}