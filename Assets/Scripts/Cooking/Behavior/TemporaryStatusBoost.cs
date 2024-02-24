using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class TemporaryStatusBoost : ICookingEffect
{
    [SerializeField]
    private float _duration;
    [SerializeField]
    private PlayerStatus _playerStatusAmount;
    [SerializeField]
    private WeaponStatus _weaponStatusAmount;

    private float TimeScale => GameSpeedManager.Instance.TimeScale;

    public IEnumerator RunAsync()
    {
        Debug.Log("バフ開始");

        var playerBuff = _playerStatusAmount;
        var weaponBuff = _weaponStatusAmount;

        PlayerController.Current.PlayerStatusEffects.Add(playerBuff);
        PlayerController.Current.WeaponStatusEffects.Add(weaponBuff);

        for (float t = 0; t < _duration; t += Time.deltaTime * TimeScale)
        {
            yield return null;
        }

        PlayerController.Current.PlayerStatusEffects.Remove(playerBuff);
        PlayerController.Current.WeaponStatusEffects.Remove(weaponBuff);

        Debug.Log("バフ終了");
    }
}