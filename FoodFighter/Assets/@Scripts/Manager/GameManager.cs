using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Experimental.GraphView;
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
    TotalCriticalDamage,
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

    private TMP_Text GoldText;
    private TMP_Text DiamondText;
    private TMP_Text TotalAtkText;

    private void Awake()
    {

        InitPlayerState();

        GoldText = GameObject.Find(Define.GoldText)?.GetComponent<TMP_Text>();
        DiamondText = GameObject.Find(Define.DiamondText)?.GetComponent<TMP_Text>();
        TotalAtkText = GameObject.Find(Define.TotalAtkText)?.GetComponent<TMP_Text>();

        OnPlayerInfoChanged += UpdateMoney;
        UpdateMoney();
    }

    private void Start()
    {
        TotalAttack();
    }

    protected override void Clear()
    {
        base.Clear();

        UpdateMoney();
    }

    void InitPlayerState() // 플레이어 스탯 초기값
    {
        this[PlayerStat.Atk] = 100;
        this[PlayerStat.CurrentHp] = 500;
        this[PlayerStat.MaxHp] = this[PlayerStat.CurrentHp];
        this[PlayerStat.CriticalProbability] = 0;
        this[PlayerStat.CriticalDamage] = 0;
        this[PlayerStat.SlotCount] = 6;
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

    // 다이아몬드 감소 함수
    public bool MinusDiamond(int amount)
    {
        if (_playerInfo.Diamond < amount) return false;
        _playerInfo.Diamond -= amount;
        OnPlayerInfoChanged?.Invoke();
        return true;
    }

    void UpdateMoney()
    {
        if (GoldText != null)
            GoldText.text = Utils.FormatKoreanNumber(_playerInfo.Gold);
        if (DiamondText != null)
            DiamondText.text = Utils.FormatKoreanNumber(_playerInfo.Diamond);
    }

    public float TotalAttack()
    {
        this[PlayerStat.TotalAtk] = this[PlayerStat.Atk] + FoodData.Instance.GetFood(SlotController.Instance.MaxLevelRef).AttackPower;
        // 기본 크리티컬 데미지(총 공격력*1.5)에서 증가한 크리티컬 데미지 %를 곱한 값을 총 공격력에 더하기
        this[PlayerStat.TotalCriticalDamage] = (this[PlayerStat.TotalAtk] * 1.5f) + ((this[PlayerStat.TotalAtk] * 1.5f) * (this[PlayerStat.CriticalDamage] / 100));

        float rand = UnityEngine.Random.Range(0f, 100f); // 0에서 100 사이의 랜덤한 수. 소수점 포함
        float damage;

        if (rand <= this[PlayerStat.CriticalProbability])
        {
            // 크리티컬 데미지 적용
            damage = this[PlayerStat.TotalCriticalDamage];
            Debug.Log($"크리티컬 발생! 현재 크리티컬 확률 : {this[PlayerStat.CriticalProbability]} 데미지 : {damage}");
        }
        else
        {
            // 총 공격력으로 데미지 적용
            damage = this[PlayerStat.TotalAtk];
        }

        TotalAtkText.text = Utils.FormatKoreanNumber((long)this[PlayerStat.TotalAtk]);

        return damage;
    }
    #endregion

    #region StageLevel
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
    #endregion
}
