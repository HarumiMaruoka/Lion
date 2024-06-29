using System;
using System.Collections.Generic;
using UnityEngine;

namespace Lion.LevelManagement
{
    /// <summary>
    /// �A�C�e��������ă��x���A�b�v����@�\��񋟂���N���X�B
    /// </summary>
    /// <typeparam name="T"> 
    /// �X�e�[�^�X��\���\���́BIStatus���������Ă���K�v������B
    /// </typeparam>
    public class ItemLevelManager<T> : LevelManager<T>, IItemLevelManager where T : IStatus
    {
        /// <summary>
        /// Key�̓��x���AValue�̓��x���A�b�v�ɕK�v�ȃA�C�e���Ƃ��̌���\���\���̂̃��X�g�B
        /// </summary>
        public Dictionary<int, List<LevelUpCost>> LevelUpCostTable { get; private set; }

        public ItemLevelManager(TextAsset levelUpCostTable, TextAsset statusTable)
        {
            CurrentLevel = 1;

            var input = levelUpCostTable.LoadCsv(1);
            LevelUpCostTable = new Dictionary<int, List<LevelUpCost>>();

            for (int i = 0; i < input.Length; i++)
            {
                var level = int.Parse(input[i][0]);
                var itemID = int.Parse(input[i][1]);
                var amount = int.Parse(input[i][2]);

                if (!LevelUpCostTable.ContainsKey(level))
                {
                    LevelUpCostTable[level] = new List<LevelUpCost>();
                }

                LevelUpCostTable[level].Add(new LevelUpCost()
                {
                    ItemID = itemID,
                    Amount = amount,
                });
            }

            input = statusTable.LoadCsv(1);
            StatusTable = new T[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                var row = input[i];
                T data = default; // �����ŃG���[����������ꍇ�́AT�^���\���̂��m�F���Ă��������B�������where T : struct���g���Ȃ��̂ŁB
                data.LoadItemSheet(row);
                StatusTable[i] = data;
            }
        }

        public List<LevelUpCost> GetLevelUpCosts(int level)
        {
            return LevelUpCostTable[level];
        }


        public string GetCurrentStatusText()
        {
            return GetStatusText(CurrentLevel);
        }

        public string GetStatusText(int level)
        {
            return StatusTable[level - 1].ToString();
        }
    }

    public struct LevelUpCost
    {
        public int ItemID { get; set; }
        public int Amount { get; set; }
    }
}