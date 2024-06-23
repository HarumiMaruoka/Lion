using System;
using UnityEngine;

public static class TextAssetExtensions
{
    public static string[][] LoadCsv(this TextAsset csvTextAsset, int ignoreRowCount = 0, int ignoreColumnCount = 0)
    {
        string[] costRows = csvTextAsset.text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
        int costRowCount = costRows.Length;
        string[][] costColumns = new string[costRowCount][];

        for (int i = ignoreRowCount; i < costRowCount; i++)
        {
            costColumns[i] = costRows[i].Split(',', StringSplitOptions.RemoveEmptyEntries);
            costColumns[i] = costColumns[i][ignoreColumnCount..costColumns[i].Length];
        }

        return costColumns[ignoreRowCount..];
    }
}