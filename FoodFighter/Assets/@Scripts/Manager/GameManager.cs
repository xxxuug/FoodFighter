using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public enum PlayerStat
{
    Atk, // 공격력
    CurrentHp, // 현재 HP
    MaxHp, // 최대 HP
    CriticalProbability, // 크리티컬 확률
    CriticalDamage, // 크리티컬 데미지
    SlotCount, // 머지 슬롯 칸 개수
    TotalAtk, // 총 공격력
}

public class PlayerInfo
{
    public int Gold;
    public int Diamond;
}

// 게임 세이브/로드
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

    void InitPlayerState() // 플레이어 스탯 초기값
    {
        this[PlayerStat.Atk] = 1;
        this[PlayerStat.CurrentHp] = 500;
        this[PlayerStat.MaxHp] = this[PlayerStat.CurrentHp];
        this[PlayerStat.CriticalProbability] = 0;
        this[PlayerStat.CriticalDamage] = 0;
        this[PlayerStat.SlotCount] = 6;
        //this[PlayerStat.TotalAtk] = 데미지 합산 값
    }
    #endregion

    #region Player Stat
    public event Action OnPlayerStatChanged;

    // 스탯 이름을 키, 수치를 값으로 저장하는 구조
    private Dictionary<PlayerStat, float> _stat = new();

    // 인덱서
    public float this[PlayerStat stat]
    {
        // _stat 딕셔너리에 stat 키가 있는지 확인 후 있으면 value, 없으면 기본값(0) 반환
        get => _stat.TryGetValue(stat, out var value) ? value : 0;
        // _stat 딕셔너리에 stat을 키로 value를 저장
        set
        {
            _stat[stat] = value;
            OnPlayerStatChanged?.Invoke();
        }
    }
    #endregion

    #region Player Info (Gold / Diamond)
    // player info 갱신
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

    // 골드 증가 함수
    public void AddGold(int gold)
    {
        _playerInfo.Gold += gold;
        //Debug.Log("현재 골드 : " + _playerInfo.Gold);
        OnPlayerInfoChanged?.Invoke();
    }

    // 다이아몬드 증가 함수
    public void AddDiamond(int diamond)
    {
        _playerInfo.Diamond += diamond;
        OnPlayerInfoChanged?.Invoke();
    }

    // 골드 감소 함수
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
    public int ClearedStageLevel = 0; // 마지막으로 클리어한 스테이지
    public int StageUnlock = 0; // 도전 버튼 누른 보스 번호

    public bool isStageUnlocked(int stageIndex)
    {
        return ClearedStageLevel >= stageIndex;
    }

    // 클리어
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
