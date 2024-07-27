using System;
using UnityEngine;

namespace Lion.Ally.Skill
{
    public class SebasSkill : SkillBase
    {
        [SerializeField]
        private Funnel _fannelPrefab;

        private void Start()
        {
            var count = 5;

            for (int i = 0; i < count; i++)
            {
                var fannel = Instantiate(_fannelPrefab, transform.position, Quaternion.identity);
                fannel.Owner = Owner;
            }
        }
    }
}