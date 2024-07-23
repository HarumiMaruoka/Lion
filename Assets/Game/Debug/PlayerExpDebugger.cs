using Lion.Player;
using System;
using UnityEngine;

namespace Lion.LionDebugger
{
    public class PlayerExpDebugger : MonoBehaviour
    {
        [SerializeField]
        private TMPro.TextMeshProUGUI _text;

        private void Show()
        {
            //_text.text = "";
            //for (int level = 1; level <= PlayerManager.Instance.ExpLevelManager.MaxLevel; level++)
            //{
            //    _text.text += $"Level {level}, ";
            //    _text.text += $"Exp: {PlayerManager.Instance.ExpLevelManager.ExpTable[level - 1]}, ";
            //    _text.text += $"Status: {PlayerManager.Instance.ExpLevelManager.StatusTable[level - 1].ToString()}\n";
            //}
        }
    }
}