using UnityEngine;
using System;

public enum MoneyType
{
    Gold,
    Diamond
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