using System;
using UnityEngine;

public static class CSVExtensions
{
    public static string[][] ParseCsv(this string str, int ignoreRows = 0)
    {
        ignoreRows = ignoreRows > 0 ? ignoreRows : 0; // マイナスの数値の場合0として扱う。

        string[][] result;

        var lines = str.Split("\r\n", StringSplitOptions.RemoveEmptyEntries);

        var allLineCount = lines.Length;
        var loadLineCount = allLineCount - ignoreRows;

        // 読み込む行が存在しない場合エラーを投げる。
        if (loadLineCount <= 0) throw new ArgumentException();

        result = new string[loadLineCount][];

        for (int i = ignoreRows; i < allLineCount; i++)
        {
            result[i - ignoreRows] = lines[i].Split(',');
        }
        return result;
    }
}