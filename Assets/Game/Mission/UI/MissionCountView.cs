using System;
using UnityEngine;

namespace Lion.Mission.UI
{
    public class MissionCountView : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _remainCountLabel;
        [SerializeField]
        private GameObject _normalIcon;

        [SerializeField]
        private TMPro.TextMeshProUGUI _fightLabel;
        [SerializeField]
        private GameObject _fightIcon;

        private void Start()
        {
            if (!MainMission.Instance)
            {
                // Debug.LogError("MainMission is not exists.");
                gameObject.SetActive(false);
                return;
            }
            MainMission.Instance.OnKillCountChanged += OnKillCountChanged;
            OnKillCountChanged(MainMission.Instance.KillCount);
        }

        private void OnDestroy()
        {
            if (MainMission.Instance != null)
            {
                MainMission.Instance.OnKillCountChanged -= OnKillCountChanged;
            }
        }

        private void OnKillCountChanged(int killCount)
        {
            var targetKillCount = MainMission.Instance.TargetKillCount;
            var remainKillCount = targetKillCount - killCount;

            if (remainKillCount <= 0)
            {
                _normalIcon.SetActive(false);
                _remainCountLabel.gameObject.SetActive(false);

                _fightIcon.SetActive(true);
                _fightLabel.gameObject.SetActive(true);

                _fightLabel.text = "FIGHT!";
            }
            else
            {
                _normalIcon.SetActive(true);
                _remainCountLabel.gameObject.SetActive(true);

                _fightIcon.SetActive(false);
                _fightLabel.gameObject.SetActive(false);

                _remainCountLabel.text = remainKillCount.ToString();
            }
        }
    }
}