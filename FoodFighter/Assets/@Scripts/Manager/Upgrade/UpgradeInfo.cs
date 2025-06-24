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
    public float Increasecost; // ��� ������
    public Sprite Icon; // ������
    public float IncreaseNum; // ���׷��̵� ���� (�󸶾� ������ �Ǵ���) 
    public MoneyType MoneyType; // ��� or ���̾�

    public PlayerStat StateType;
} 