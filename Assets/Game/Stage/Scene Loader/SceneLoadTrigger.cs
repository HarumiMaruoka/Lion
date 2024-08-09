using Lion.Home;
using Lion.Player;
using Lion.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lion.SceneManagement
{
    public class SceneLoadTrigger : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName;

        private bool _isPlayerInTrigger = false;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != PlayerController.Instance.gameObject) return;
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