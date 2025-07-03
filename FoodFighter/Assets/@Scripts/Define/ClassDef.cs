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
        public LockType lockType; // � �������� ������ (���ݷ� or ��������)    
        public int Level; // �䱸 ����
        public int StageIndex;
        public GameObject LockObject; // ������� �� ������ ������Ʈ
    }

    //public class PlayerInfo
    //{
    //    public int Gold;
    //    public int Diamond;
    //}

    // ���� ���̺�/�ε�
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
        public string Name; // ��ȭ�̸�
        public float Cost; // ���
        public float IncreaseCost; // ��� ������
        public float IncreaseNum; // ���׷��̵� ���� (�󸶾� ������ �Ǵ���) 
        public MoneyType MoneyType; // ��� or ���̾�

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
