using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Glib.NovelGameEditor.Scenario.Commands
{
    public static class NovelCanvasActions
    {
        public static async UniTask ShowNovelCanvas(Config config, string[] commandArgs)
        {
            float fadeDuration = 0f;
            if (commandArgs == null || commandArgs.Length < 1 || !float.TryParse(commandArgs[0], out float value)) fadeDuration = 0.5f;
            else fadeDuration = value;

            config.NovelCanvas.blocksRaycasts = true;
            for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
            {
                config.NovelCanvas.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                try
                {
                    await UniTask.Yield(config.NovelCanvas.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }
            config.NovelCanvas.alpha = 1f;
        }

        public static async UniTask HideNovelCanvas(Config config, string[] commandArgs)
        {
            float fadeDuration = 0f;
            if (commandArgs == null || commandArgs.Length < 1 || !float.TryParse(commandArgs[0], out float value)) fadeDuration = 0.5f;
            else fadeDuration = value;

            for (float t = 0f; t < fadeDuration; t += Time.deltaTime)
            {
                config.NovelCanvas.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                try
                {
                    await UniTask.Yield(config.NovelCanvas.GetCancellationTokenOnDestroy());
                }
                catch (OperationCanceledException)
                {
                    return;
                }
            }

            config.NovelCanvas.blocksRaycasts = false;
            config.NovelCanvas.alpha = 0f;
        }
    }
}