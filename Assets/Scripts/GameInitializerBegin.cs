using System;
using UnityEngine;

[DefaultExecutionOrder(-90)]
public class GameInitializerBegin : MonoBehaviour
{
    private void Awake()
    {
        // ‚¿•¨î•ñ‚Ì‰Šú‰»B
        ItemInventory.Instance.Initialize();
    }
}