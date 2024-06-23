using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Glib.NovelGameEditor.Scenario.Commands.Background
{
    public class BackgroundActions
    {
        public static async UniTask RunAction(Config config, string[] commandArgs)
        {
            var actionType = commandArgs[0].Trim();

            switch (actionType)
            {
                case "Change": await ChangeBackgroundImage(config, commandArgs[1..]); break;
                default: Debug.Log($"{actionType} is missing."); break;
            }
        }

        public static async UniTask ChangeBackgroundImage(Config config, string[] args)
        {
            var spriteName = args[0];
            var duration = float.Parse(args[1]);

            var frontView = config.BackgroundFront;
            var backView = config.BackgroundBack;

            var sprite = config.FindBackgroundSprite(spriteName);

            backView.sprite = frontView.sprite;
            frontView.sprite = sprite;

            frontView.color = new Color(1, 1, 1, 0);
            backView.color = new Color(1, 1, 1, 1);

            try
            {
                await frontView.FadeAsync(1, duration, frontView.GetCancellationTokenOnDestroy());
                backView.color = new Color(1, 1, 1, 0);
            }
            catch (OperationCanceledException)
            {
                return;
            }
        }
    }
}