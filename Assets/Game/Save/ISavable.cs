using System;
using UnityEngine;

namespace Lion.Save
{
    public interface ISavable
    {
        void Save();
        void Load();
    }
}