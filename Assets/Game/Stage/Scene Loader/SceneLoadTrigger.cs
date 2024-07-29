using Lion.Player;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lion.SceneManagement
{
    public class SceneLoadTrigger : MonoBehaviour
    {
        [SerializeField]
        private string _sceneName;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject != PlayerController.Instance.gameObject) return;
            SceneManager.LoadScene(_sceneName);
        }
    }
}