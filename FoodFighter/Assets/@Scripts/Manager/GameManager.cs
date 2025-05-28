using System;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerStat
{
    Atk, // ���ݷ�
    CurrentHp, // ���� HP
    MaxHp, // �ִ� HP
    CriticalProbability, // ũ��Ƽ�� Ȯ��
    CriticalDamage, // ũ��Ƽ�� ������
    SlotCount, // ���� ���� ĭ ����
    TotalAtk, // �� ���ݷ�
}

public class PlayerInfo
{
    public int Gold;
    public int Diamond;
}

public class StageInfo
{
    public int MainStage;
    public int SubStage;

    public string GetDisplayStage()
    {
        return $"{MainStage} - {SubStage}";
    }
}

// ���� ���̺�/�ε�
[Serializable]
public class GameData
{
    public PlayerInfo PlayerInfo;
    public StageInfo StageInfo;
    public Dictionary<PlayerStat, float> stat;
}

public class GameManager : Singleton<GameManager>
{
    #region Init
    private void Awake()
    {
        InitPlayerState();
    }

    void InitPlayerState() // �÷��̾� ���� �ʱⰪ
    {
        this[PlayerStat.Atk] = 0;
        this[PlayerStat.CurrentHp] = 50;
        this[PlayerStat.MaxHp] = this[PlayerStat.CurrentHp];
        this[PlayerStat.CriticalProbability] = 0;
        this[PlayerStat.CriticalDamage] = 0;
        this[PlayerStat.SlotCount] = 6;
        //this[PlayerStat.TotalAtk] = ������ �ջ� ��
    }
    #endregion

    #region Player Stat
    public event Action OnPlayerStatChanged;

    // ���� �̸��� Ű, ��ġ�� ������ �����ϴ� ����
    private Dictionary<PlayerStat, float> _stat = new();

    // �ε���
    public float this[PlayerStat stat]
    {
        // _stat ��ųʸ��� stat Ű�� �ִ��� Ȯ�� �� ������ value, ������ �⺻��(0) ��ȯ
        get => _stat.TryGetValue(stat, out var value) ? value : 0;
        // _stat ��ųʸ��� stat�� Ű�� value�� ����
        set => _stat[stat] = value;
    }
    #endregion

    #region Player Info (Gold / Diamond)
    // player info ����
    public event Action OnPlayerInfoChanged;

    private PlayerInfo _playerInfo = new PlayerInfo()
    {
        Gold = 0,
        Diamond = 0,
    };

    public PlayerInfo PlayerInfo
    {
        get { return _playerInfo; }
        set
        {
            _playerInfo = value;
            OnPlayerInfoChanged?.Invoke();
        }
    }

    // ��� ���� �Լ�
    public void AddGold(int gold)
    {
        _playerInfo.Gold += gold;
        OnPlayerInfoChanged?.Invoke();
    }

    // ���̾Ƹ�� ���� �Լ�
    public void AddDiamond(int diamond)
    {
        _playerInfo.Diamond += diamond;
        OnPlayerInfoChanged?.Invoke();
    }
    #endregion

    #region Stage Info
    public event Action OnStageInfoChanged;

    private StageInfo _stageInfo = new StageInfo()
    {
        MainStage = 1,
        SubStage = 1,
    };

    public StageInfo StageInfo
    {
        get { return _stageInfo; }
        set
        {
            _stageInfo = value;
            OnStageInfoChanged?.Invoke();
        }
    }
    #endregion
}
