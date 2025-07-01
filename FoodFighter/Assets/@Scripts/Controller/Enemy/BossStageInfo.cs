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
        public int Stage; // �� ��° ���� ������������
        public GameObject BossPrefab; // ���� ������
        public float CurrentHp;
        public float Damage;
        public int RewardGold;
        public int RewardDiamond;

        public Vector3 SpawnPosition; // ���� ���� ��ġ
        public Vector3 TargetPosition; // ������ �̵��ؾ� �� �߾� ��ġ
    }

    public List<Data> list = new List<Data>();
}