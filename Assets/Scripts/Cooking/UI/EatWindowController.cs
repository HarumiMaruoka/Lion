using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EatWindowController : MonoBehaviour
{
    [SerializeField]
    private GameObject _confirmationWindow;
    [SerializeField]
    private CookingController _cookingController;

    [SerializeField]
    private Toggle _confirmationSkipToggle;

    [SerializeField]
    private Text _confirmationLabel;
    [SerializeField]
    private Button _confirmationYesButton;
    [SerializeField]
    private Button _confirmationNoButton;

    private bool _isButtonClicked = false;
    private bool _isYesButtonClicked = false;

    private void Start()
    {
        // 確認ウィンドウを非表示にする。
        _confirmationWindow.SetActive(false);

        // Yes Buttonの初期化。
        _confirmationYesButton.onClick.AddListener(
            () => StartCoroutine(OnConfirmationYesButtonClicked()));

        // No Buttonの初期化。
        _confirmationNoButton.onClick.AddListener(
            () => StartCoroutine(OnConfirmationNoButtonClicked()));
    }

    public void EatFoodRequest(int selectedFoodID)
    {
        StartCoroutine(EatFoodAsync(selectedFoodID));
    }

    private IEnumerator EatFoodAsync(int selectedFoodID)
    {
        var food = CookingFoodDataBase.Current.GetData(selectedFoodID);

        if (_confirmationSkipToggle.isOn) // 確認をスキップする。
        {
            _cookingController.EatFood(food);
            yield break;
        }
        // 確認ウィンドウを表示する。
        _confirmationWindow.SetActive(true);
        _confirmationLabel.text = $"{food.Name} を使用しますか？";
        yield return WaitButtonClicked(); // 確認を待機する。
        if (_isYesButtonClicked)
        {
            _cookingController.EatFood(food);
        }
        // 確認ウィンドウを非表示にする。
        _confirmationWindow.SetActive(false);
    }

    private IEnumerator WaitButtonClicked()
    {
        while (!_isButtonClicked)
        {
            yield return null;
        }
    }

    private IEnumerator OnConfirmationYesButtonClicked()
    {
        _isButtonClicked = true;
        _isYesButtonClicked = true;

        yield return null;

        _isButtonClicked = false;
        _isYesButtonClicked = false;
    }

    private IEnumerator OnConfirmationNoButtonClicked()
    {
        _isButtonClicked = true;

        yield return null;

        _isButtonClicked = false;
    }
}