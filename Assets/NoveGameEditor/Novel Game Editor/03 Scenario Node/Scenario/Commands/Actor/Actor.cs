using System;
using UnityEngine;
using UnityEngine.UI;

namespace Glib.NovelGameEditor.Scenario.Commands.ActorActions
{
    public class Actor : MonoBehaviour
    {
        public ActorType ActorType;
        public Sprite[] Sprites;
        public Animator Animator;
        public Image ActorFrontView;
        public Image ActorBackView;
        public RectTransform RectTransform;
        public Reaction[] Reactions;

        // 現時点の状態を保存する。
        private ActorState _buffer;
        public void SaveState()
        {
            _buffer = new ActorState
            {
                FrontSprite = ActorFrontView.sprite,
                BackSprite = ActorBackView.sprite,
                Position = RectTransform.anchoredPosition,
            };
        }

        // 保存した状態に戻す。
        public void LoadState()
        {
            ActorFrontView.sprite = _buffer.FrontSprite;
            ActorBackView.sprite = _buffer.BackSprite;
            RectTransform.anchoredPosition = _buffer.Position;
        }

        public struct ActorState
        {
            public Sprite FrontSprite;
            public Sprite BackSprite;
            public Vector2 Position;
        }
    }
}

public enum ActorType
{
    アリス,
    ボブ,
}