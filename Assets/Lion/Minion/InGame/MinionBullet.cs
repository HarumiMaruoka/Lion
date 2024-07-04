using System;
using UnityEngine;

namespace Lion.Minion
{
    public class MinionBullet : MonoBehaviour
    {
        public MinionController Controller { get; set; }
        public MinionStatus Status => Controller.Status;
    }
}