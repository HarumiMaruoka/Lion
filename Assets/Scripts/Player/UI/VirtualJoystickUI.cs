using System;
using UnityEngine;
using UnityEngine.UI;

public class VirtualJoystickUI : MonoBehaviour
{
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Image _foreground;
    [SerializeField]
    private float _maxRadius;

    private Vector2 _beginPosition;

    private void Update()
    {
        if (PlayerController.Current.SelectUICount != 0)
        {
            return;
        }

        if (Input.touchCount == 1)
        {
            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                Begin(touch.position);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                End();
            }
            ExecuteWhileActive(touch.position);
        }
    }

    public void Begin(Vector2 position)
    {
        _beginPosition = position;

        _background.enabled = true;
        _foreground.enabled = true;

        _background.rectTransform.position = position;
        _foreground.rectTransform.position = position;
    }

    public void ExecuteWhileActive(Vector2 position)
    {
        if ((_beginPosition - position).sqrMagnitude < _maxRadius * _maxRadius)
            _foreground.rectTransform.position = position;
        else
            _foreground.rectTransform.position = _beginPosition + (position - _beginPosition).normalized * _maxRadius;
    }

    public void End()
    {
        _background.enabled = false;
        _foreground.enabled = false;
    }
}