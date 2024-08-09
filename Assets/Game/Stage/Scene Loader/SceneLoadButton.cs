using Lion.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Lion.SceneManagement
{
    [RequireComponent(typeof(Button))]
    public class SceneLoadButton : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName;
        private bool _isPlayerInTrigger = false;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnButtonClick);
        }

        private void OnButtonClick()
        {
            if (_isPlayerInTrigger) return;
            _isPlayerInTrigger = true;
            ScreenFader.Instance.FadeIn(onComplete: LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}