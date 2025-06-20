using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Main {
    public class DefaultUIButton: InteractableUIBase {

        [Header("Target")]
        [SerializeField] private TMP_Text _context;
        [SerializeField] private GameObject _object;
        [FormerlySerializedAs("_addScale")]
        [Space]
        [Header("Parameter")]
        [SerializeField] private float _addScaleRatio;
        [SerializeField] private Color _targetColor;
        [Header("Enter")]
        [SerializeField] private float _enterDuration;
        [SerializeField] private Ease _enterAnimationType;
        [Header("Exit")]
        [SerializeField] private float _exitDuration;
        [SerializeField] private Ease _exitAnimationType;
        
        private Vector2 _defaultScale;
        private Color _defaultColor;

        private new void Awake() {
            base.Awake();
            
            _defaultColor = _context.color;
            _defaultScale = _object.transform.localScale;
        }

        protected override Tween OnPointerEnter() =>
            _object.transform
                .DOScale(_defaultScale * _addScaleRatio, _enterDuration)
                .SetEase(_enterAnimationType)
                .OnStart(() => _context.color = _targetColor);

        protected override Tween OnPointerExit() => 
            _object.transform
                .DOScale(_defaultScale, _exitDuration)
                .SetEase(_exitAnimationType)
                .OnStart(() => _context.color = _defaultColor);
    }
} 