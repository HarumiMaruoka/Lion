using System;
using UnityEngine;
using UnityEngine.UI;

public class ActorStatusWindow : MonoBehaviour
{
    [SerializeField]
    private Text _title;
    [SerializeField]
    private Text _content;

    private IActor _actor;

    public IActor Actor
    {
        get => _actor;
        set
        {
            _actor = value;
            _title.text = value.Name;
            _content.text = $"Level: {value.Level}\n" + value.Status.ToString();
        }
    }
}