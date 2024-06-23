using System;
using UnityEditor;
using UnityEngine;

public class ExcelToCsvSettings : ScriptableObject
{
    [SerializeField]
    //[HideInInspector]
    private string _excelFolderPath;
    [SerializeField]
    //[HideInInspector]
    private string _csvFolderPath;

    public string ExcelFolderPath => _excelFolderPath;
    public string CsvFolderPath => _csvFolderPath;

    public void RecordExcelFolderPath(string path)
    {
        _excelFolderPath = path;
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }

    public void RecordCsvFolderPath(string path)
    {
        _csvFolderPath = path;
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    }
}