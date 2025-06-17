using System;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField] private TMP_Text GoldText;
    [SerializeField] private TMP_Text DiamondText;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        InitPlayerState();

        GoldText = GameObject.Find("Gold Text - Text")?.GetComponent<TMP_Text>();
        DiamondText = GameObject.Find("Diamond Text - Text")?.GetComponent<TMP_Text>();

        OnPlayerInfoChanged += UpdateMoney;
        UpdateMoney();
    }

    void InitPlayerState() // �÷��̾� ���� �ʱⰪ
    {
        this[PlayerStat.Atk] = 1;
        this[PlayerStat.CurrentHp] = 500;
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
        set
        {
            _stat[stat] = value;
            OnPlayerStatChanged?.Invoke();
        }
    }
    #endregion

    #region Player Info (Gold / Diamond)
    // player info ����
    public event Action OnPlayerInfoChanged;

    private PlayerInfo _playerInfo = new PlayerInfo()
    {
        Gold = 1000,
        Diamond = 50,
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
        //Debug.Log("���� ��� : " + _playerInfo.Gold);
        OnPlayerInfoChanged?.Invoke();
    }

    // ���̾Ƹ�� ���� �Լ�
    public void AddDiamond(int diamond)
    {
        _playerInfo.Diamond += diamond;
        OnPlayerInfoChanged?.Invoke();
    }

    // ��� ���� �Լ�
    public bool MinusGold(int amount)
    {
        if (_playerInfo.Gold < amount) return false;
        _playerInfo.Gold -= amount;
        OnPlayerInfoChanged?.Invoke();
        return true;
    }

    public bool MinusDiamond(int amount)
    {
        if (_playerInfo.Diamond < amount) return false;
        _playerInfo.Diamond -= amount;
        OnPlayerInfoChanged?.Invoke();
        return true;
    }

    void UpdateMoney()
    {
        //GoldText.text = $"{Gold}";
        //DiamondText.text = $"{Diamond}";

        if (GoldText != null)
        GoldText.text = $"{PlayerInfo.Gold}";
        if (DiamondText != null)
        DiamondText.text = $"{PlayerInfo.Diamond}";
    }
    #endregion

    #region StageLavel
    public int ClearedStageLevel = 0; // ���������� Ŭ������ ��������
    public int StageUnlock = 0; // ���� ��ư ���� ���� ��ȣ

    public bool isStageUnlocked(int stageIndex)
    {
        return ClearedStageLevel >= stageIndex;
    }

    // Ŭ����
    public void ClearStage(int stageIndex)
    {
        if (stageIndex > ClearedStageLevel)
        {
            ClearedStageLevel = stageIndex;
        }
    }

    //public List<bool> StageLocked;

    //public void UnlockStage(int stageIndex)
    //{
    //    StageLocked[stageIndex] = false;
    //}

    //public bool IsStageLocked(int stageIndex)
    //{
    //    return StageLocked[stageIndex];
    //}
    #endregion
}
