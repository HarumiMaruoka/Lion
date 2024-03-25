using System;
using UnityEngine;

namespace Character
{
    public interface ISkillUser
    {
        int SkillID { get; }
        CharacterIndividualData IndividualData { get; }
        ActorStatus Status { get; }
        Transform Transform { get; }
    }
}