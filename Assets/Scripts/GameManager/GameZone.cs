using System;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class GameZone : MonoBehaviour
{
    [SerializeField]
    private Transform _topLeft;
    [SerializeField]
    private Transform _bottomRight;

    public float LeftLimit => _topLeft.position.x;
    public float RightLimit => _bottomRight.position.x;
    public float TopLimit => _topLeft.position.y;
    public float BottomLimit => _bottomRight.position.y;

    private static GameZone _current;
    public static GameZone Current => _current;

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

public static class Utility
{
    public static bool IsInGameZone(this MonoBehaviour monoBehaviour)
    {
        var pos = monoBehaviour.transform.position;
        var gameZone = GameZone.Current;

        if (pos.x > gameZone.RightLimit || pos.x < gameZone.LeftLimit ||
            pos.y > gameZone.TopLimit || pos.y < gameZone.BottomLimit)
        {
            return false;
        }

        return true;
    }
}