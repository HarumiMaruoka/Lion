using System;
using UnityEngine;

namespace Lion.LevelManagement
{
    public interface IItemLevelable
    {
        IItemLevelManager ItemLevelManager { get; }

        bool IsActive { get; }
        event Action<bool> OnActiveChanged;

        Sprite ActorSprite { get; }
        Sprite IconSprite { get; }
    }
}