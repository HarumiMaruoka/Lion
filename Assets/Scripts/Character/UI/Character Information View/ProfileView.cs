using System;
using UnityEngine;
using UnityEngine.UI;

public class ProfileView : MonoBehaviour
{
    [SerializeField]
    private Text _caption;
    [SerializeField]
    private Text _content;

    public Text Caption => _caption;
    public Text Content => _content;

    internal void Invalid()
    {
        throw new NotImplementedException();
    }
}