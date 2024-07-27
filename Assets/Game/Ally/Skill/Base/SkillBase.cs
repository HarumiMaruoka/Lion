using System;
using UnityEngine;

namespace Lion.Ally.Skill
{
    public class SkillBase : MonoBehaviour
    {
        public IActor Owner { get; set; }
    }
}