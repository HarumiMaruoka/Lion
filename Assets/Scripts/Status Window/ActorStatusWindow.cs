using System;
using UnityEngine;
using UnityEngine.UI;

public class ActorStatusWindow : MonoBehaviour
{
    [SerializeField]
    private Text _label;

    private ActorStatus _actorStatus;

    public ActorStatus ActorStatus
    {
        get => _actorStatus;
        set
        {
            _actorStatus = value;
            _label.text = value.ToString();
        }
    }
}