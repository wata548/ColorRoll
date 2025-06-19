using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
    public abstract class InteractableUIBase: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

        public static readonly Dictionary<string, bool> _groupInfo = new();

        public static void SetGroup(string target, bool @switch) =>
            _groupInfo[target] = @switch;
        
        [SerializeField] private string _group;
        private Tween _curAnimation = null;
        private bool _isOn = false;
        
        protected abstract Tween OnPointerEnter();
        protected abstract Tween OnPointerExit();
        
        public void OnPointerEnter(PointerEventData eventData) {

            if (!_groupInfo[_group])
                return;

            _isOn = true;
            if (_curAnimation != null)
                _curAnimation.Kill();
            
            _curAnimation = OnPointerEnter();
        }

        public void OnPointerExit(PointerEventData eventData) {

            _isOn = false;
            if (_curAnimation != null)
                _curAnimation.Kill();
            
            _curAnimation = OnPointerExit();
        }

        protected void Awake() {
            _groupInfo.TryAdd(_group, true);
        }

        protected void Update() {
            //Not focusing
            if (_isOn && !_groupInfo[_group])
                OnPointerExit(null);
        }
    }
}