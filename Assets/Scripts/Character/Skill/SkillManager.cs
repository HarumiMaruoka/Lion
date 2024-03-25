using Character.Skill;
using System;
using UnityEngine;

namespace Character
{
    public static class SkillManager
    {
        // もったいないので同じインスタンスを使いまわす。
        private static ISkillBehavior _sampleSkill1 = new SampleSkill1();
        private static ISkillBehavior _sampleSkill2 = new SampleSkill2();
        private static ISkillBehavior _sampleSkill3 = new SampleSkill3();
        private static ISkillBehavior _sampleSkill4 = new SampleSkill4();

        public static ISkillBehavior GetSkillBehavior(int skillID)
        {
            return skillID switch
            {
                1 => _sampleSkill1,
                2 => _sampleSkill2,
                3 => _sampleSkill3,
                4 => _sampleSkill4,
                _ => null,
            };
        }
    }
}