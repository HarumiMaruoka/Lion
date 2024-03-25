using System;
using UnityEngine;

namespace Character
{
    public interface ISkill
    {
        string Name { get; }
        string Description { get; }
        ISkillBehavior Behavior { get; }
    }
}