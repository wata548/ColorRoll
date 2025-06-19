using TMPro;
using UnityEngine;

namespace UI.Main {
    public class MakeRoomModal: MonoBehaviour {
        [SerializeField] private TMP_InputField _input;
        public string Context => _input.text;

        public void Init() => _input.text = "";
        public void SetActive(bool active) => gameObject.SetActive(active);
    }
}