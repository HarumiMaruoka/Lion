using System;
using System.Linq;
using System.Threading;
using UnityEngine;

public class CookingController : MonoBehaviour
{
    private int[] _selectedMaterialIDs = Enumerable.Repeat(-1, 3).ToArray();
    private Action<int>[] _onSelectedMaterialChangeds = new Action<int>[3];

    public int[] SelectedMaterialIDs => _selectedMaterialIDs;
    public Action<int>[] OnSelectedChangeds => _onSelectedMaterialChangeds;

    /// <summary>
    /// 選択された料理素材をフィールドに保存する。
    /// </summary>
    /// <param name="selectedMaterialID"> 選択された料理素材のID。 </param>
    public void SelectMaterial(int selectedMaterialID)
    {
        for (int i = 0; i < _selectedMaterialIDs.Length; i++)
        {
            if (_selectedMaterialIDs[i] == -1)
            {
                _selectedMaterialIDs[i] = selectedMaterialID;
                _onSelectedMaterialChangeds[i]?.Invoke(selectedMaterialID);

                SelectedMaterialChanged();

                return;
            }
        }
        Debug.Log("これ以上選択できない為、選択されたオブジェクトは無効。");
    }

    /// <summary>
    /// 選択を解除する。
    /// </summary>
    /// <returns> もともと選択されていた料理素材のID。 </returns>
    public int ExcludeMaterial(int index)
    {
        var old = _selectedMaterialIDs[index];
        _selectedMaterialIDs[index] = -1;
        _onSelectedMaterialChangeds[index]?.Invoke(-1);

        SelectedMaterialChanged();
        return old;
    }

    private CookingFoodData _makableFood = null;

    public CookingFoodData MakableFood => _makableFood;

    public Action<CookingFoodData> OnMakableFoodChanged;

    private void SelectedMaterialChanged()
    {
        var foodData = CookingFoodDataBase.Current.Data;

        foreach (var food in foodData)
        {
            if (food.Equal(SelectedMaterialIDs))
            {
                _makableFood = food;
                OnMakableFoodChanged?.Invoke(food);
                return;
            }
        }

        _makableFood = null;
        OnMakableFoodChanged?.Invoke(null);
    }

    public void MakeFood(bool isDeselect = true)
    {
        // 料理を作れるかどうかチェック。
        if (_makableFood == null)
        {
            Debug.Log("制作可能な料理がありません。");
            return;
        }
        if (!CookingMaterialInventory.Instance.VerifyInventory(_makableFood))
        {
            // フラグに応じて、全ての選択済み料理素材を解除する。
            for (int i = 0; _selectedMaterialIDs.Length > i; i++)
            {
                if (isDeselect) ExcludeMaterial(i);
            }
            return;
        }

        // 料理インベントリに料理を追加する。
        CookingFoodInventory.Instance.Add(_makableFood.ID);

        // 選択済み料理素材をインベントリから減らし、
        // フラグに応じて、全ての選択済み料理素材を解除する。
        for (int i = 0; _selectedMaterialIDs.Length > i; i++)
        {
            CookingMaterialInventory.Instance.Use(_selectedMaterialIDs[i]);
            if (isDeselect) ExcludeMaterial(i);
        }
    }

    private CancellationTokenSource _foodEffectCancellationTokenSource = null;

    public void EatFood(CookingFoodData food)
    {
        // 実行中であれば、そのコルーチンを停止し、新しいコルーチンを開始する。
        _foodEffectCancellationTokenSource?.Cancel();
        _foodEffectCancellationTokenSource = new CancellationTokenSource();

        var cookingEffect = food.CookingEffect;
        if (cookingEffect != null)
        {
            StartCoroutine(food.CookingEffect.RunAsync(_foodEffectCancellationTokenSource.Token));
        }

        CookingFoodInventory.Instance.Use(food.ID);
    }

    public void EatFood(int foodID)
    {
        EatFood(CookingFoodDataBase.Current.GetData(foodID));
    }
}