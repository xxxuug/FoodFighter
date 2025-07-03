using UnityEngine;

namespace EnumDef
{
    // ����
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
        Atk, // ���ݷ�
        CurrentHp, // ���� HP
        MaxHp, // �ִ� HP
        CriticalProbability, // ũ��Ƽ�� Ȯ��
        CriticalDamage, // ũ��Ƽ�� ������
        SlotCount, // ���� ���� ĭ ����
        TotalAtk, // �� ���ݷ�
        TotalCriticalDamage,

        Max
    }

    public enum LockType
    {
        AttackLevel, // ���ݷ� ����
        Stage, // �������� ���� 
    }

    public enum MoneyType
    {
        Gold,
        Diamond
    }

}
