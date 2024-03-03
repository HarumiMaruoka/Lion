using System;
using UnityEngine;

public interface IProfile
{
    bool IsValid { get; }
    string Caption { get; }
    string ContentedText { get; }
}