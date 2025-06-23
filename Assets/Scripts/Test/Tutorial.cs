using System;
using System.Collections.Generic;
using DG.Tweening;
using MapInfo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Test {
    public class Tutorial: MonoBehaviour {

        [SerializeField] private Text _text1;
        [SerializeField] private InputTest _player;
        [SerializeField] private Image _disappear;

        private Tween TweenGenerator(List<string> tutorial, Action callBack = null) {

            _text1.text = "";
            
            //970 -> 850
            var result = DOTween.Sequence();
            foreach (var d in tutorial) {
                result.Append(_text1.DOText(d, 2))
                    .AppendInterval(1f)
                    .AppendCallback(() => _text1.text = "");
            }

            result.OnComplete(() => callBack?.Invoke());

            return result;
        }

        private int _level = 0;
        private bool _isFirst = true;
        private int _breakCount = 0;
        
        private void Update() {
            switch (_level) {
                case 0:
                    if (_isFirst) {
                        _isFirst = false;
                        
                        TweenGenerator(new() {
                            "안녕하십니까?",
                            "ColorRoll 튜토리얼에 오신 걸 환영합니다.",
                            "먼저 움직여봅시다.\n WASD를 눌러보세요"
                        }, () => _level++);
                    }

                    break;
                case 1:
                    if (_player.Movement >= 20) {
                        
                        _level++;
                        _isFirst = true;
                    }
                    _text1.text = $"WASD로 움직이자\n({(int)_player.Movement}/20)";
                    break;
                case 2: 
                    if (_isFirst) {
                        _isFirst = false;
                        
                        TweenGenerator(new() {
                            "잘 하셨습니다.",
                            "이제 차징을 해봅시다.",
                            "Shift를 눌러 차징해 봅시다.",
                            "길게 누를 수록 차징이 강해집니다.",
                            "최고 강도로 차지해 봅시다."
                        }, () => _level++);
                    }
                    break;
                
                case 3:
                    if (_player.ChargeLevel >= 2) {
                        _level++;
                        _isFirst = true;
                    }

                    _text1.text = "최고 강도로 차지해 봅시다.";
                    break;    
                case 4:
                    if (_isFirst) {
                        _isFirst = false;
                        
                        TweenGenerator(new() {
                                "잘하셨습니다.",
                                "차징을 한 뒤 벽에 부딫치면 벽이 부서집니다.",
                                "벽을 부숴 봅시다.",
                            }, () => {
                                _level++;
                                _breakCount = BlockGenerator.Instance.BreakCount;
                            }
                        );
                    }
                    break;
                case 5:
                    int count = BlockGenerator.Instance.BreakCount - _breakCount;
                    if (count >= 5) {
                        _level++;
                        _isFirst = true;
                    }

                    _text1.text = $"벽을 부숴봅시다. ({count}/5)";
                    break;
                case 6:
                    if (_isFirst) {
                        _isFirst = false;
                        TweenGenerator(new() {
                            "잘하셨습니다.",
                            "이 게임은 이런 식으로 좁은 공간에서",
                            "벽을 부숴나가며 더 넓은 공간을 ",
                            "자신의 색으로 칠한 플레이어가 승리하는 게임입니다.",
                            "이상으로 튜토리얼을 마칩니다."
                        }, () => {
                            _disappear.DOFade(1, 2)
                                .SetEase(Ease.OutCubic)
                                .OnComplete(() =>
                                SceneManager.LoadScene("Main")
                            );
                        });
                    }

                    break;
            }
        }
    }
}