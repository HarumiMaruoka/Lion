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
            public float ObservingMinTime; // 観察する最小時間
            public float ObservingMaxTime; // 観察する最大時間

            public float ObservingMaxDistance; // 観察する最大距離
            public float ObservingMinDistance; // 観察する最小距離

            public float MoveSpeed; // 移動速度
            public float ArrivalThresholdDistance; // 到着判定距離
            public float RangeAttackThresholdDistance; // 遠距離攻撃判定距離
        }
    }
}