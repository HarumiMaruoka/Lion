using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class Converter
{
    private static string relativePath = "ExcelToCsvExtension\\Editor\\ExcelToCsvAll2.exe";
    private static string fullPath = Path.Combine(Application.dataPath.Replace("/", "\\"), relativePath);

    public static void Convert()
    {
        // 実行する外部プロセスのパス
        string exePath = fullPath;

        // コマンドライン引数を組み立てる
        string arguments = $"\"{FolderSelector.SelectedExcelPath}\" \"{FolderSelector.SelectedCsvPath}\"";

        // プロセスを起動する
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        // 標準出力を読み取る
        using (Process process = Process.Start(startInfo))
        {
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            UIManager.AddLog(output);
            AssetDatabase.Refresh();
        }
    }
}