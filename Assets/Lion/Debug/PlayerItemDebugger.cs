using Lion.Player;
using System;
using UnityEngine;

namespace Lion.LionDebugger
{
    public class PlayerItemDebugger : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        private void Show()
        {
            _text.text = "";
            //for (int level = 1; level <= PlayerManager.Instance.ItemLevelManager.MaxLevel; level++)
            //{
            //    _text.text += $"Level {level}, ";
            //    for (int i = 0; i < PlayerManager.Instance.ItemLevelManager.LevelUpCostTable[level].Count; i++)
            //    {
            //        _text.text += $"Item ID: {PlayerManager.Instance.ItemLevelManager.LevelUpCostTable[level][i].ItemID}: " +
            //            $"Amount: {PlayerManager.Instance.ItemLevelManager.LevelUpCostTable[level][i].Amount}, ";
            //    }
            //    _text.text += $"Status: {PlayerManager.Instance.ItemLevelManager.GetNextStatus(level).ToString()}\n";
            //}
        }
    }
}