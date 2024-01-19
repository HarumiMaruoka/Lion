using System;
using UnityEngine;

[Serializable]
public struct UpgradeCost // 強化に掛かるコスト
{
    public UpgradeCost(int itemID, int amount)
    {
        _itemID = itemID;
        _amount = amount;
    }

    public static UpgradeCost[] Parse(string[] csvRow)
    {
        if (csvRow == null)
        {
            throw new ArgumentNullException(nameof(csvRow));
        }

        UpgradeCost[] result = new UpgradeCost[csvRow.Length / 2];

        for (int i = 0; i < csvRow.Length; i += 2)
        {
            var cost = new UpgradeCost();
            // ExcelからCsvに変換した際に数値に小数点が含まれている場合があるので floatで Parseする。
            // intにキャストして小数点以下を切り捨てて利用する。
            if (!float.TryParse(csvRow[i], out float idResult) ||
                !float.TryParse(csvRow[i + 1], out float amountResult))
            {
                throw new FormatException("Invalid data format. Unable to parse.");
            }

            cost._itemID = (int)idResult;
            cost._amount = (int)amountResult;

            result[i / 2] = cost;
        }

        return result;
    }

    [SerializeField]
    private int _itemID;
    [SerializeField]
    private int _amount;

    public int ItemID => _itemID;
    public int Amount => _amount;
}