using System;
using UnityEngine;

public struct DummyProfile : IProfile
{
    public bool IsValid => true;

    public string Caption => "Dummy Profile";

    public string ContentedText => "Dummy Profile\nDummy Profile\nDummy Profile\nDummy Profile\nDummy Profile\nDummy Profile\n";
}