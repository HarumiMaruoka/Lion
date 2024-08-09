using Lion.Formation;
using System;
using UnityEngine;

// 戦闘の資格をチェックするクラス
namespace Lion.Home
{
    public class CombatEligibilityChecker
    {
        public static bool IsEligibleForCombat
        {
            get
            {
                // 同行者がいない場合は戦闘不可
                if (FormationManager.Instance.FrontlineAlly == null) return false;
                // 同行者が武器を一つも装備していない場合は戦闘不可
                if (FormationManager.Instance.FrontlineAlly.Equipped(0) == null &&
                    FormationManager.Instance.FrontlineAlly.Equipped(1) == null &&
                    FormationManager.Instance.FrontlineAlly.Equipped(2) == null &&
                    FormationManager.Instance.FrontlineAlly.Equipped(3) == null) return false;

                return true;
            }
        }
    }
}