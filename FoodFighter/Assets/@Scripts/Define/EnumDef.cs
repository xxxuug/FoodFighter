using UnityEngine;

namespace EnumDef
{
    // 전투
    public enum BattleState
    {
        None,
        MoveToCenter,
        WaitTurn,
        PlayerTurn,
        BossTurn,
        End
    }

    public enum PlayerStat
    {
        Atk, // 공격력
        CurrentHp, // 현재 HP
        MaxHp, // 최대 HP
        CriticalProbability, // 크리티컬 확률
        CriticalDamage, // 크리티컬 데미지
        SlotCount, // 머지 슬롯 칸 개수
        TotalAtk, // 총 공격력
        TotalCriticalDamage,

        Max
    }

    public enum LockType
    {
        AttackLevel, // 공격력 레벨
        Stage, // 스테이지 레벨 
    }

    public enum MoneyType
    {
        Gold,
        Diamond
    }

}
