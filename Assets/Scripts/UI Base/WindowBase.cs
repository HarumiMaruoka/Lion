using System;
using UnityEngine;

public class WindowBase : MonoBehaviour
{
    // 各ウィンドウは、Windowオブジェクトのアクティブ、非アクティブでOpen、Closeの表現を行う。
    private static int _openedWindowCount = 0;

    public static int OpenedWindowCount => _openedWindowCount;

    public static event Action<int> OnOpenedWindowCountChanged;

    protected virtual void OnEnable() // Windowを開いたときに呼び出す。
    {
        _openedWindowCount++;
        OnOpenedWindowCountChanged?.Invoke(_openedWindowCount);
    }

    protected virtual void OnDisable() // Windowを閉じたときに呼び出す。
    {
        _openedWindowCount--;
        OnOpenedWindowCountChanged?.Invoke(_openedWindowCount);
    }

    public static void ClearCount()
    {
        _openedWindowCount = 0;
        // OnOpenedWindowCountChanged += value => Debug.Log(value);
        OnOpenedWindowCountChanged?.Invoke(_openedWindowCount);
    }
}