using System;
using UnityEngine;

namespace Lion.Minion.UI
{
    public class Row : MonoBehaviour
    {
        [field: SerializeField] public MinionIcon Left { get; private set; }
        [field: SerializeField] public MinionIcon Right { get; private set; }

        public void UpdateUI()
        {
            Left.UpdateUI();
            Right.UpdateUI();
        }
    }
}