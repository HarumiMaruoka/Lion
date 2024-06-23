using Cysharp.Threading.Tasks;
using Glib.NovelGameEditor.Scenario.Commands;
using Glib.NovelGameEditor.Scenario.Commands.ActorActions;
using Glib.NovelGameEditor.Scenario.Commands.Background;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Glib.NovelGameEditor.Scenario
{
    [Serializable]
    public class ScenarioRunner
    {
        [SerializeField] private TextAsset _scenarioData;

        public bool IsFinished { get; private set; } = false;

        public void RunScenario(Config config)
        {
            List<List<Command>> commands = ParseCommands(_scenarioData.text, config);
            RunCommands(commands);
        }

        private async void RunCommands(List<List<Command>> commands)
        {
            IsFinished = false;

            for (int i = 0; i < commands.Count; i++)
            {
                UniTask[] tasks = new UniTask[commands[i].Count];
                for (var j = 0; j < commands[i].Count; j++)
                {
                    tasks[j] = commands[i][j].CommandFunction(commands[i][j].Config, commands[i][j].CommandArgs);
                }
                await UniTask.WhenAll(tasks);

#if UNITY_EDITOR
                if (!UnityEditor.EditorApplication.isPlaying) return;
#endif
            }

            IsFinished = true;
        }

        private static List<List<Command>> ParseCommands(string input, Config config)
        {
            List<List<Command>> result = new List<List<Command>>();

            string pattern = @"\[(.*?)\]";
            MatchCollection matches = Regex.Matches(input, pattern);

            List<Command> group = new List<Command>();
            bool groupFlag = false;

            foreach (Match match in matches)
            {
                string parameter = match.Groups[1].Value;
                string[] commandLine = parameter.Split(',');

                string commandName = commandLine[0];

                if (commandName == "BeginGroup")
                {
                    group = new List<Command>();
                    result.Add(group);
                    groupFlag = true;
                }
                else if (commandName == "EndGroup")
                {
                    groupFlag = false;
                }
                else if (groupFlag)
                {
                    group.Add(new Command
                    {
                        CommandFunction = GetCommandFunction(commandName),
                        Config = config,
                        CommandArgs = commandLine[1..]
                    });
                }
                else
                {
                    group = new List<Command>();
                    result.Add(group);

                    group.Add(new Command
                    {
                        CommandFunction = GetCommandFunction(commandName),
                        Config = config,
                        CommandArgs = commandLine[1..]
                    });
                }
            }

            return result;
        }

        private static Func<Config, string[], UniTask> GetCommandFunction(string commandName)
        {
            switch (commandName)
            {
                case "ScreenFade": return ScreenActions.FadeAsync;
                case "ChangeCaption": return CaptionActions.ChangeCaption;
                case "PrintText": return TextBoxActions.PrintText;
                case "SetText": return TextBoxActions.SetText;
                case "ActorAction": return ActorActions.RunAction;
                case "BackgroundAction": return BackgroundActions.RunAction;
                case "ShowNovelCanvas": return NovelCanvasActions.ShowNovelCanvas;
                case "HideNovelCanvas": return NovelCanvasActions.HideNovelCanvas;
                default: Debug.Log($"{commandName} not found."); return null;
            };
        }
    }

    public struct Command
    {
        public Func<Config, string[], UniTask> CommandFunction;
        public Config Config;
        public string[] CommandArgs;
    }
}