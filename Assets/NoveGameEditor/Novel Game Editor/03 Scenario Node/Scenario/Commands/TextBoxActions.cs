using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace Glib.NovelGameEditor.Scenario.Commands
{
    public class TextBoxActions
    {
        public static async UniTask SetText(Config config, string[] commandArgs)
        {
            config.TextBox.text = commandArgs[0];
        }

        public static async UniTask PrintText(Config config, string[] commandArgs)
        {
            config.NovelClickHandler.OnClicked += OnClicked;

            string text = commandArgs[0];
            float duration = float.Parse(commandArgs[1]);

            bool isSkipRequested = false;

            float autoModeTimer = 0;
            float autoModeInterval = 1f;

            try
            {
                // Duration秒掛けて1文字ずつテキストを表示する。
                var showed = "";
                for (int i = 0; i < text.Length; i++)
                {
                    showed += text[i];
                    if (!config.TextBox) return;
                    config.TextBox.text = showed;
                    for (float t = 0; t < duration / text.Length; t += Time.deltaTime)
                    {
                        await UniTask.Yield(config.TextBox.GetCancellationTokenOnDestroy());
                        // クリックしたら即座に全文表示
                        if (_isClicked)
                        {
                            _isClicked = false;
                            isSkipRequested = true;
                            break;
                        }
                    }

                    if (isSkipRequested) break;
                }

                await UniTask.Yield(config.TextBox.GetCancellationTokenOnDestroy());
                config.TextBox.text = text;

                // クリック待ち
                while (!_isClicked ||
                    autoModeTimer > autoModeInterval)
                {
                    if (config.IsAutoMode) autoModeTimer += Time.deltaTime;
                    else autoModeTimer = 0;
                    await UniTask.Yield(config.TextBox.GetCancellationTokenOnDestroy());
                }
                _isClicked = false;
            }
            catch (OperationCanceledException)
            {
                return;
            }

            config.NovelClickHandler.OnClicked -= OnClicked;
        }

        private static bool _isClicked = false;

        private static void OnClicked()
        {
            _isClicked = true;
        }
    }
}