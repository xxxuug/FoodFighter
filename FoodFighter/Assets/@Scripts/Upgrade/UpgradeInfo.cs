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
    public string name; // ��ȭ�̸�
    public int cost; // ���
    public float IncreaseNum; // ���׷��̵� ���� 
    public MoneyType moneyType; // ��� or ���̾�
}