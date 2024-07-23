using Lion.Player;
using System;
using UnityEngine;

namespace Lion.Enemy.Boss
{
    /// <summary>
    /// 近接攻撃オブジェクト。
    /// </summary>
    public class MeleeAttackObject : MonoBehaviour
    {
        public Stage1BossParameters Parameters { get; set; }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // プレイヤーに当たったらダメージを与える処理をここに書く。
        }
    }
}