using Lion.LevelManagement;
using Lion.LevelManagement.ExperienceLevel;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Player.UI
{
    public class LevelAndExpView : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _levelText = default;
        [SerializeField]
        private Slider _expSlider = default;
        [SerializeField]
        private TextMeshProUGUI _expText = default;

        private ExperienceLevelManager ExpLevelManager => PlayerManager.Instance.LevelManager.ExpLevelManager;

        private int CurrentLevel => ExpLevelManager.CurrentLevel;
        private int CurrentExp => ExpLevelManager.CurrentExp;
        private int CurrentLevelExp => ExpLevelManager.CurrentLevelExp;
        private int NextLevelExp => ExpLevelManager.NextLevelExp;

        private void Start()
        {
            UpdateLevelAndExp(default);
            ExpLevelManager.OnExpChanged += UpdateLevelAndExp;
        }

        private void OnDestroy()
        {
            ExpLevelManager.OnExpChanged -= UpdateLevelAndExp;
        }

        // イベントハンドラーに登録するためにダミー引数を追加。
        private void UpdateLevelAndExp(int dummy)
        {
            _expSlider.minValue = CurrentLevelExp;
            _expSlider.maxValue = NextLevelExp;
            _expSlider.value = CurrentExp;
            _expText.text = $"{CurrentExp}/{NextLevelExp}";

            _levelText.text = $"Lv.{CurrentLevel}";
        }
    }
}