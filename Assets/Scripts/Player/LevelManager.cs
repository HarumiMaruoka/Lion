using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class LevelManager
{
    [SerializeField]
    private Slider _slider;
    [SerializeField]
    private Text _text;

    private float _currentEXP = 0;
    private float _nextEXP = 10; // この値を超えたらレベルアップする。
    private int _level = 1;

    private Func<IEnumerator> OnLevelUpAsync;

    public int Level => _level;

    public void Initialize()
    {
        _text.text = "Level: " + _level.ToString();

        _slider.minValue = 0f;
        _slider.maxValue = _nextEXP;
        _slider.value = _currentEXP;
    }

    public IEnumerator GetGem(Gem gem)
    {
        _currentEXP += gem.EXP;
        _slider.value = _currentEXP;

        while (_currentEXP >= _nextEXP)
        {
            _currentEXP -= _nextEXP;
            _nextEXP *= 1.2f; // ちょっとずつ要求EXPを増やす。
            _level++;

            _text.text = "Level: " + _level.ToString();

            _slider.minValue = 0f;
            _slider.maxValue = _nextEXP;
            _slider.value = _currentEXP;

            yield return OnLevelUpAsync?.Invoke();
        }
    }
}