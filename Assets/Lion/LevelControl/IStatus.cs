using System;
using UnityEngine;

namespace Lion
{
    public interface IStatus
    {
        void ItemCsvLoad(string[] row);
        void ExpCsvLoad(string[] row);
    }
}