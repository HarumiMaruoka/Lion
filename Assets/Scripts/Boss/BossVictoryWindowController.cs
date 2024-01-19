using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossVictoryWindowController : MonoBehaviour
{
    [SerializeField]
    private BossManager _bossManager;
    [SerializeField]
    private Image _background;
    [SerializeField]
    private Text _text;

    private void Start()
    {
        // 背景を透明にする。
        var bgCol = _background.color;
        bgCol.a = 0f;
        _background.color = bgCol;

        // テキストを透明にする。
        var textCol = _text.color;
        textCol.a = 0f;
        _text.color = textCol;

        // テキストを割り当てる。
        var dropItemData = _bossManager.DropItemData;
        _text.text = "";
        for (int i = 0; i < dropItemData.Length; i++)
        {
            _text.text += $"ItemID: {dropItemData[i].ItemID}を、{dropItemData[i].Amount}個手に入れました。\n";
        }
    }

    [SerializeField]
    private float _inputInvalidTime; // 入力無効時間
    [SerializeField]
    private string _closeInputName;

    public IEnumerator WaitCloseRequest() // ウィンドウを閉じる要求が来るのを待機する。
    {
        // 入力無効時間が経過するのを待機する。
        for (float t = 0f; t < _inputInvalidTime; t += Time.deltaTime) yield return null;
        // 指定されたボタンが押下されるのを待機する。
        while (!Input.GetButtonDown(_closeInputName))
        {
            yield return null;
        }
    }

    [SerializeField]
    private float _showAnimationDuration;

    public Coroutine PlayShowAnimation()
    {
        StopAllCoroutines();
        return StartCoroutine(ShowAsync());
    }

    [SerializeField]
    private float _bgAlpha;

    public IEnumerator ShowAsync()
    {
        var textCoroutine = StartCoroutine(_text.FadeAsync(1f, _showAnimationDuration));
        var bgCoroutine = StartCoroutine(_background.FadeAsync(_bgAlpha, _showAnimationDuration));

        yield return textCoroutine;
        yield return bgCoroutine;
    }

    [SerializeField]
    private float _hideAnimationDuration;

    public Coroutine PlayHideAnimation()
    {
        StopAllCoroutines();
        return StartCoroutine(HideAsync());
    }

    public IEnumerator HideAsync()
    {
        var textCoroutine = StartCoroutine(_text.FadeAsync(0f, _hideAnimationDuration));
        var bgCoroutine = StartCoroutine(_background.FadeAsync(0f, _hideAnimationDuration));

        yield return textCoroutine;
        yield return bgCoroutine;
    }
}