using System;
using UnityEngine;

[DefaultExecutionOrder(-90)]
public class GameInitializerBegin : MonoBehaviour
{
    private void Awake()
    {
        // 持ち物情報の初期化。
        ItemInventory.Instance.Initialize();
    }
}