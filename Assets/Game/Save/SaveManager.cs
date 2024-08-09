using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.Save
{
    public class SaveManager
    {
        public static SaveManager Instance { get; } = new SaveManager();
        private SaveManager() { }

        private HashSet<ISavable> _savables = new HashSet<ISavable>();

        public void Register(ISavable savable)
        {
            _savables.Add(savable);
        }

        public void Unregister(ISavable savable)
        {
            _savables.Remove(savable);
        }

        public void Save()
        {
            foreach (var savable in _savables)
            {
                savable.Save();
            }
        }

        public void Load()
        {
            foreach (var savable in _savables)
            {
                savable.Load();
            }
        }
    }
}