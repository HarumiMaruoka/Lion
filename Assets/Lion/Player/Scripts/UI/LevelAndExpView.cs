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

        private ExpLevelManager<Status> PlayerExpManager => PlayerManager.Instance.ExpLevelManager;

        private void Start()
        {
            UpdateLevelAndExp(PlayerExpManager.Exp);
            PlayerExpManager.OnExpChanged += UpdateLevelAndExp;
        }

        private void OnDestroy()
        {
            PlayerExpManager.OnExpChanged -= UpdateLevelAndExp;
        }

        private void UpdateLevelAndExp(int exp)
        {
            _expSlider.minValue = PlayerExpManager.GetLevelExp();
            _expSlider.maxValue = PlayerExpManager.GetNextLevelExp();
            _expSlider.value = PlayerExpManager.Exp;
            _levelText.text = PlayerExpManager.Level.ToString();

            _expText.text = $"{PlayerExpManager.Exp}/{PlayerExpManager.GetNextLevelExp()}";
        }
    }
}