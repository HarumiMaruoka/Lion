using System;
using UnityEngine;

namespace Lion.LevelManagement
{
    public interface IStatus
    {
        void ItemCsvLoad(string[] row);
        void ExpCsvLoad(string[] row);
    }
}