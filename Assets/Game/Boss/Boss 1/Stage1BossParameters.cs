using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// ボスのステージ1のパラメータ。
    /// </summary>
    public class Stage1BossParameters : MonoBehaviour
    {
        public float MaxLife;
        public IdleStateParameters IdleState;
        public ApproachingStateParameters ApproachingState;
        public RetreatStateParameters RetreatState;
        public ObservingStateParameters ObservingState;

        // 待機ステートのパラメータ
        [Serializable]
        public class IdleStateParameters
        {
            public float WaitMinTime;
            public float WaitMaxTime;
        }

        // プレイヤーに近づくステートのパラメータ
        [Serializable]
        public class ApproachingStateParameters
        {
            public float ArrivalThresholdDistance;
            public float MoveSpeed;
        }

        // プレイヤーと離れるステートのパラメータ
        [Serializable]
        public class RetreatStateParameters
        {
            public float ArrivalThresholdDistance;
            public float MoveSpeed;
        }

        // 観察ステートのパラメータ
        [Serializable]
        public class ObservingStateParameters
        {
            public float ObservingMinTime;
            public float ObservingMaxTime;

            public float ObservingMaxDistance;
            public float ObservingMinDistance;

            public float MoveSpeed;
            public float ArrivalThresholdDistance;
            public float RangeAttackThresholdDistance;
        }
    }
}