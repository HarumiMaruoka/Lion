using System;
using System.Collections;
using UnityEngine;

public class CharacterAppreciationWindow : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _dirtParticle;

    private bool _isDirtParticleStopRequested = false;

    public void CreateDirtParticle()
    {

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