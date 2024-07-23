using System;
using UnityEngine;

namespace Lion.Ally.UI
{
    public class Row : MonoBehaviour
    {
        [field: SerializeField] public AllyIcon Left { get; private set; }
        [field: SerializeField] public AllyIcon Right { get; private set; }

        public void UpdateUI()
        {
            Left.UpdateUI();
            Right.UpdateUI();
        }
    }
}