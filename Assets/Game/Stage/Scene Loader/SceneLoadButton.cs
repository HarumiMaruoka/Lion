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

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(_sceneName));
        }
    }
}