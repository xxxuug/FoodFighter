using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossStageInfo", menuName = "Scripts/BossStageInfo", order = 1)]
public class BossStageInfo : ScriptableObject
{
    [Serializable]
    public class Data
    {
        public int Stage; // 몇 번째 보스 스테이지인지
        public GameObject BossPrefab; // 보스 프리팹
        public float CurrentHp;
        public float Damage;
        public int RewardGold;
        public int RewardDiamond;

        public Vector3 SpawnPosition; // 보스 시작 위치
        public Vector3 TargetPosition; // 보스가 이동해야 할 중앙 위치
    }

    public List<Data> list = new List<Data>();
}