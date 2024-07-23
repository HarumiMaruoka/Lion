using System;
using UnityEngine;

namespace Lion.LevelManagement
{
    public interface IStatus
    {
        int Level { get; }
        void LoadStatusFromCsv(string[] csv);
    }
}