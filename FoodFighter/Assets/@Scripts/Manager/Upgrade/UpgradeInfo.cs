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
    public string Name; // 강화이름
    public float Cost; // 비용
    public float Increasecost; // 비용 증가율
    public Sprite Icon; // 아이콘
    public float IncreaseNum; // 업그레이드 증가 (얼마씩 증가가 되는지) 
    public MoneyType MoneyType; // 골드 or 다이아

    public PlayerStat StateType;
} 