using Lion.Enemy;
using Lion.Gem;
using Lion.Gold;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.LionDebugger
{
    public class ObjectCountView : MonoBehaviour
    {
        [SerializeField] private Text _view;
        [SerializeField] private ObjectType _objectType;

        private void Update()
        {
            switch (_objectType)
            {
                case ObjectType.Enemy:
                    UpdateEnemyCount();
                    break;
                case ObjectType.Gem:
                    UpdateGemCount();
                    break;
                case ObjectType.Gold:
                    UpdateGoldCount();
                    break;
            }
        }

        private void UpdateEnemyCount()
        {
            _view.text = "Enemy Count: " + EnemyManager.Instance.EnemyPool.ActiveCount.ToString();
        }

        private void UpdateGemCount()
        {
            _view.text = "Gem Count: " + DroppedGemPool.Instance.ActiveCount.ToString();
        }

        private void UpdateGoldCount()
        {
            _view.text = "Gold Count: " + DroppedGoldPool.Instance.ActiveCount.ToString();
        }

        public enum ObjectType
        {
            Enemy,
            Gem,
            Gold,
        }
    }
}