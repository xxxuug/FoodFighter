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
    public Sprite Icon; // ������
    public float IncreaseNum; // ���׷��̵� ���� (�󸶾� ������ �Ǵ���) 
    public MoneyType moneyType; // ��� or ���̾�
} 