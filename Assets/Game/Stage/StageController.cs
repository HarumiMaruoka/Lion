using System;
using UnityEngine;

namespace Lion.Stage
{
    public class StageController : MonoBehaviour
    {
        [SerializeField]
        private bool _isBattleScene = false;

        private void Awake()
        {
            StageManager.Instance.IsBattleScene = _isBattleScene;
        }
    }
}