using System;
using System.Collections;
using UnityEngine;

public class CharacterAppreciationView : MonoBehaviour
{
    [SerializeField]
    private CharacterInventoryWindow _inventoryWindow;

    public void ShowCharacterSelectWindow()
    {

    }

    public void HideCharacterSelectWindow()
    {

    }

    [SerializeField]
    private CharacterAppreciationWindow _appreciationWindow;

    public void ShowAppreciationWindow(CharacterIndividualData selectedCharacter)
    {

    }

    public void HideAppreciationWindow()
    {

    }

    [SerializeField]
    private ParticleSystem _dirtParticle;

    private bool _isDirtParticleStopRequested = false;

    public void StartDirtParticle(Action<float> dirtinessLevelGetter)
    {
        _isDirtParticleStopRequested = false;

        StartCoroutine(DrawDirtParticleAsync(dirtinessLevelGetter));
    }

    public void StopDirtParticle()
    {
        _isDirtParticleStopRequested = true;
    }

    private IEnumerator DrawDirtParticleAsync(Action<float> dirtinessLevelGetter)
    {
        while (!_isDirtParticleStopRequested)
        {
            yield return null;
        }
    }

    [SerializeField]
    private ParticleSystem _bubbleParticle;

    public void StartBubbleParticle()
    {
        _bubbleParticle.Play();
    }

    public void StopBubbleParticle()
    {
        _bubbleParticle.Stop();
    }

    [SerializeField]
    private ParticleSystem _heartParticle;

    public void StartHeartParticle()
    {
        _heartParticle.Play();
    }

    public void StopHeartParticle()
    {
        _heartParticle.Stop();
    }
}