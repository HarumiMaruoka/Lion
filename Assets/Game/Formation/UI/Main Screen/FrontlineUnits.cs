using Lion.Ally;
using Lion.Minion;
using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Formation.UI.Frontline
{
    public class FrontlineUnits : MonoBehaviour
    {
        [SerializeField]
        private PlayerIcon _playerIcon;
        [SerializeField]
        private AllyIcon _allyIcon;
        [SerializeField]
        private MinionIcon[] _minionIcons;

        private void Start()
        {
            // 初期値の設定。
            ApplyPlayerIcon();
            ApplyAllyIcon(FormationManager.Instance.FrontlineAlly);
            for (int i = 0; i < _minionIcons.Length; i++)
            {
                ApplyMinionIcon(i, FormationManager.Instance.GetFrontlineMinion(i));
            }

            // 変更イベントに登録。
            FormationManager.Instance.OnAllyChanged += ApplyAllyIcon;
            FormationManager.Instance.OnMinionChanged += ApplyMinionIcon;
        }

        private void OnDestroy()
        {
            // 変更イベントを解除。
            FormationManager.Instance.OnAllyChanged -= ApplyAllyIcon;
            FormationManager.Instance.OnMinionChanged -= ApplyMinionIcon;
        }

        private void ApplyPlayerIcon()
        {
            _playerIcon.Image.sprite = PlayerManager.Instance.Icon;
        }

        private void ApplyAllyIcon(AllyData ally)
        {
            if (ally == null)
            {
                _allyIcon.Image.sprite = null;
                _allyIcon.Image.color = Color.clear;
                return;
            }
            _allyIcon.Image.sprite = ally.Icon;
            _allyIcon.Image.color = Color.white;
        }

        private void ApplyMinionIcon(int index, MinionData minion)
        {
            if (minion == null)
            {
                _minionIcons[index].gameObject.SetActive(false);
                return;
            }

            _minionIcons[index].gameObject.SetActive(true);
            _minionIcons[index].Image.sprite = minion.Icon;
            _minionIcons[index].Image.color = Color.white;
        }
    }
}