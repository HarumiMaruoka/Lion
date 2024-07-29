using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Utility
{
    [RequireComponent(typeof(Image))]
    public class ScreenFade : MonoBehaviour
    {
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }
    }
}