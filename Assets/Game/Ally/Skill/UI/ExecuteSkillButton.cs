using Lion.Formation;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Lion.Ally.Skill
{
    [RequireComponent(typeof(Button))]
    public class ExecuteSkillButton : MonoBehaviour
    {
        private AllyController Ally => FormationManager.Instance?.FrontlineAlly?.Instance;

        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(ExecuteSkill);
        }

        private void ExecuteSkill()
        {
            Ally?.ExecuteSkill();
        }
    }
}