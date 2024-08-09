using Lion.UI;
using System;
using UnityEngine;

namespace Lion.Stage
{
    [DefaultExecutionOrder(-1000)]
    public class StageController : MonoBehaviour
    {
        [SerializeField]
        private bool _isBattleScene = false;

        private void Awake()
        {
            StageManager.Instance.IsBattleScene = _isBattleScene;
        }

        private void Start()
        {
            ScreenFader.Instance.FadeOut();
        }
    }
}