using System;
using System.Collections.Generic;
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

public class StageInfo
{
    public int MainStage;
    public int SubStage;

    public string GetDisplayStage()
    {
        return $"{MainStage} - {SubStage}";
    }
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
    private void Awake()
    {
        InitPlayerState();
    }

    void InitPlayerState() // 플레이어 스탯 초기값
    {
        this[PlayerStat.Atk] = 0;
        this[PlayerStat.CurrentHp] = 50;
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
        set => _stat[stat] = value;
    }
    #endregion

    #region Player Info (Gold / Diamond)
    // player info 갱신
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

    // 골드 증가 함수
    public void AddGold(int gold)
    {
        _playerInfo.Gold += gold;
        OnPlayerInfoChanged?.Invoke();
    }

    // 다이아몬드 증가 함수
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
