using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameWindowArea : MonoBehaviour
{
    [SerializeField]
    private Transform _topLeft;
    [SerializeField]
    private Transform _bottomRight;

    public Transform TopLeft => _topLeft;
    public Transform BottomRight => _bottomRight;

    public float Left => _topLeft.position.x;
    public float Right => _bottomRight.position.x;
    public float Top => _topLeft.position.y;
    public float Bottom => _bottomRight.position.y;

    private static GameWindowArea _current;
    public static GameWindowArea Current => _current;

    private void Awake()
    {
        if (_current)
        {
            Debug.LogError("Already exists. Have you placed two or more?");
        }
        _current = this;
    }

    private void OnDestroy()
    {
        _current = null;
    }
}