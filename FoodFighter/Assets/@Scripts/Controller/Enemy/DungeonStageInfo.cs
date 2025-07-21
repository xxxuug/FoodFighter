using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonStageInfo", menuName = "Scripts/DungeonStageInfo", order = 1)]
public class DungeonStageInfo : ScriptableObject
{
    [Serializable]
    public class Data
    {
        public string DungeonName; // 골드, 다이아
        public int Stage;
        public GameObject BossPrefab;
        public float MaxHP;
        public float CurrentHP;
       // public int RewardGold;
       // public int RewardDiamond;
        public Sprite Icon;
        public int TimeLimit;
       // public int TicketCount; // 입장권 필요 수량
    }

    public List<Data> list = new();
}