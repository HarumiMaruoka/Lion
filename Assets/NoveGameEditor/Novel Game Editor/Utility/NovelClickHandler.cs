using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Glib.NovelGameEditor.Utility
{
    public class NovelClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public event Action OnClicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            OnClicked?.Invoke();
        }
    }
}