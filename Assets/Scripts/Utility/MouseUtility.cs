using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class MouseUtility
{
    public static List<RaycastResult> GetMouseOverUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        Vector2 mousePosition = Input.mousePosition;
        eventData.position = mousePosition;

        List<RaycastResult> result = new();
        EventSystem.current.RaycastAll(eventData, result);
        return result;
    }

    private static int _lastFrameCount = -1; // 最後に _mouseOverUIBuffer に書きこんだフレーム。より良い命名募集中。
    private static List<RaycastResult> _mouseOverUIBuffer = new List<RaycastResult>();

    public static List<RaycastResult> GetMouseOverUINonAlloc()
    {
        if (Time.frameCount == _lastFrameCount) // 同じフレームで毎回計算する必要はないのでキャッシュした値を返す。
        {
            return _mouseOverUIBuffer;
        }

        // 以下 異なるフレームでこのメソッドが呼び出された場合。
        _lastFrameCount = Time.frameCount;

        // ここ毎回 newしたくないのでアイデア募集中。
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        Vector2 mousePosition = Input.mousePosition;
        eventData.position = mousePosition;

        EventSystem.current.RaycastAll(eventData, _mouseOverUIBuffer);
        return _mouseOverUIBuffer;
    }

    public static bool IsMouseOverUI()
    {
        return GetMouseOverUINonAlloc().Count != 0;
    }
}