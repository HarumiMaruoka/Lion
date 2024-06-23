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
        // ���s����O���v���Z�X�̃p�X
        string exePath = fullPath;

        // �R�}���h���C��������g�ݗ��Ă�
        string arguments = $"\"{FolderSelector.SelectedExcelPath}\" \"{FolderSelector.SelectedCsvPath}\"";

        // �v���Z�X���N������
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = exePath,
            Arguments = arguments,
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        // �W���o�͂�ǂݎ��
        using (Process process = Process.Start(startInfo))
        {
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            UIManager.AddLog(output);
            AssetDatabase.Refresh();
        }
    }
}