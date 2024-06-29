using System;
using UnityEngine;

namespace Lion.LevelManagement
{
    public interface IStatus
    {
        void LoadExpSheet(string[] row);
        void LoadItemSheet(string[] row);
    }
}