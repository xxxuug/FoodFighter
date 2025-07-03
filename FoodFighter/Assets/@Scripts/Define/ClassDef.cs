using EnumDef;
using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace ClassDef
{
    [Serializable]
    public class LockInfo
    {
        public LockType lockType; // 어떤 조건으로 잠기는지 (공격력 or 스테이지)    
        public int Level; // 요구 레벨
        public int StageIndex;
        public GameObject LockObject; // 잠겨있을 때 보여줄 오브젝트
    }

    //public class PlayerInfo
    //{
    //    public int Gold;
    //    public int Diamond;
    //}

    // 게임 세이브/로드
    [Serializable]
    public class GameData
    {
        //public PlayerInfo PlayerInfo;
        public StageInfo StageInfo;
        public Dictionary<PlayerStat, float> stat;
    }

    [Serializable]
    public class UpgradeInfo
    {
        public string Name; // 강화이름
        public float Cost; // 비용
        public float IncreaseCost; // 비용 증가율
        public float IncreaseNum; // 업그레이드 증가 (얼마씩 증가가 되는지) 
        public MoneyType MoneyType; // 골드 or 다이아

        public PlayerStat StateType;
    }

    public class StageInfo
    {
        public int MainStage;
        public int SubStage;

        public string GetDisplayStage()
        {
            return $"STAGE {MainStage}-{SubStage}";
        }
    }

    public class FoodSlotInfo
    {
        public Vector2Int indexColRow;
        public int foodLevel = 0;
        public bool isLock = true;

        public FoodSlotInfo(int _colIndex, int _rowIndex)
        {
            indexColRow.x = _colIndex;
            indexColRow.y = _rowIndex;
        }
    }
}
