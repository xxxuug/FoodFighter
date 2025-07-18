using ClassDef;
using EnumDef;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    const int MAX_BOSS_STAGE_COUNT = 7; // 총 보스 던전 횟수
    #region Init

    public int CurBossStageIndex = 0; // 현재 보스 스테이지

    bool[] _bossStageOpenStateArr = new bool[MAX_BOSS_STAGE_COUNT];
    public bool[] BossStageOpen
    {
        get => _bossStageOpenStateArr;
        set => _bossStageOpenStateArr = value;  
    }

    //private TMP_Text GoldText;
    //private TMP_Text DiamondText;
    //private TMP_Text TotalAtkText;
    public int Gold { get; set; } = 0;
    public int Diamond { get; set; } = 500000;

    public BossStageInfo bossStageInfo { get; set; }
    public int AttackLevel { get; set; } // 공격력 레벨

    public FoodSlotInfo[] foodSlotInfoArr = new FoodSlotInfo[6*5];

    public bool isFirstFoodSpawn = false;

    private void Start()
    {

        LoadLastStage(); // 마지막 스테이지 복구
        CheckOfflineReward(); // 오프라인 보상 체크

        LoadUpgradeDate();
        LoadSlotData();

        LoadGoldDiaData();

        LoadBossStageUnlock();
    }


    // 어플리케이션이 완전히 종료될 때 호출됨
    private void OnApplicationQuit()
    { 
        SaveQuitData(); 
        SaveSlotData();
    }

    // 어플이 백그라운드로 가거나 일시 정지 되었을 때 호출됨
    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveQuitData(); // 앱이 일시정지 될 때도 시간과 스테이지 저장
            SaveSlotData();
        }
    }

    // 게임 종료 또는 일시정지 시 데이터 저장
    private void SaveQuitData()
    {
        // 현재 시간을 이진(Binary) 형태로 문자열 저장
        PlayerPrefs.SetString(Define.LastPlayTimeKey, DateTime.Now.ToBinary().ToString());

        // 골드 & 다이아몬드 저장
        PlayerPrefs.SetInt("Gold", Gold);
        PlayerPrefs.SetInt("Diamond", Diamond);
       
        // 현재 스테이지 정보를 저장
        if (StageManager.Instance != null)
        {
            PlayerPrefs.SetInt(Define.MainStageKey, StageManager.Instance.StageInfo.MainStage);
            PlayerPrefs.SetInt(Define.SubStageKey, StageManager.Instance.StageInfo.SubStage);
        }

        PlayerPrefs.Save();
    }

    // 게임 재시작 시 마지막 스테이지 불러오기
    private void LoadLastStage()
    {
        // 저장된 스테이지 정보가 있는지 확인
        if (PlayerPrefs.HasKey(Define.MainStageKey) && PlayerPrefs.HasKey(Define.SubStageKey))
        {
            // 저장된 Main/Suub 값 불러옴
            int main = PlayerPrefs.GetInt(Define.MainStageKey);
            int sub = PlayerPrefs.GetInt(Define.SubStageKey);

            // StageManager의 현재 StageInfo에 복구된 값 세팅
            StageManager.Instance.StageInfo = new StageInfo
            {
                MainStage = main,
                SubStage = sub
            };

            Debug.Log($"복구됨 Main: {main}, Sub: {sub}");
        }
    }

    // 오프라인 보상을 계산하고 팝업으로 보여주는 함수
    private void CheckOfflineReward()
    {
        // 저장된 종료 시간이 없다면 return
        if (!PlayerPrefs.HasKey(Define.LastPlayTimeKey)) return;

        // Convert.ToInt64: long 정수형으로 바꿔주는 함수
        // 저장된 문자열을 다시 DateTime으로 변환
        long binaryTime = Convert.ToInt64(PlayerPrefs.GetString(Define.LastPlayTimeKey));
        DateTime lastPlay = DateTime.FromBinary(binaryTime); // DateTime: 날짜 + 시간을 다루는 구조체

        // TimeSpan: 두 날짜/시간 간의 차이(시간 간격)을 나타내는 구조체
        // 현재 시각과 종료 시각의 차이 계산
        TimeSpan elapsed = DateTime.Now - lastPlay;

        // 경과 시간(초) x 10 = 총 보상 골드
        // 1초(초당) 10 골드 씩
        int rewardGold = Mathf.FloorToInt((float)elapsed.TotalSeconds * 10f);
        if (rewardGold <= 0) return; // 0 이하라면 보상 x

        UI_OfflineReward.Instance.Show(rewardGold, elapsed);

        // 스테이지 보상 
        // 1분당 1스테이지 진행
        int stageUpCount = Mathf.FloorToInt((float)elapsed.TotalMinutes / 1f);

        //올라갈 스테이지가 하나라도 있다면
        if (stageUpCount > 0)
        {
            AdvanceStage(stageUpCount); // 계산된 스테이지 수만큼 실제로 스테이지 진행
            Debug.Log($"오프라인 진행 {stageUpCount} 스테이지 자동 진행");

        }
    }

    // 지정된 수(count) 만큼 스테이지를 앞으로 진행시키는 함수
    private void AdvanceStage(int count)
    {
        for (int i = 0; i < count; i++)
        {
            // 현재 SubStage가 5 이상이면 메인 스테이지를 올리고 SubStage는 1로 초기화
            if (StageManager.Instance.StageInfo.SubStage >= 5)
            {
                StageManager.Instance.StageInfo.MainStage++;
                StageManager.Instance.StageInfo.SubStage = 1;
            }
            else
            {
                StageManager.Instance.StageInfo.SubStage++; // SubStage만 증가
            }
        }
    }

    // 강화 데이터 저장
    public void SaveUpgradeDate()
    {
        foreach (var pair in _level)
        {
            string key = $"UpgradeLevel_{pair.Key}";
            PlayerPrefs.SetInt(key, pair.Value);
        }

        foreach(var pair in _stat)
        {
            string key = $"UpgradeStat_{pair.Key}";
            PlayerPrefs.SetFloat(key, pair.Value);
        }

        PlayerPrefs.SetInt("AttackLevel", AttackLevel);

        PlayerPrefs.Save();
    }

    // 강화 데이터 불러오기
    void LoadUpgradeDate()
    {
        foreach(PlayerStat stat in Enum.GetValues(typeof(PlayerStat)))
        {
            string levelKey = $"UpgradeLevel_{stat}";
            string statKey = $"UpgradeStat_{stat}";

            if (PlayerPrefs.HasKey(levelKey))
                _level[stat] = PlayerPrefs.GetInt(levelKey);

            if (PlayerPrefs.HasKey(statKey))
                _stat[stat] = PlayerPrefs.GetFloat(statKey);
        }

        if (PlayerPrefs.HasKey("AttackLevel"))
            AttackLevel = PlayerPrefs.GetInt("AttackLevel");
    }

    void SaveSlotData()
    {
        for (int i = 0; i < foodSlotInfoArr.Length; i++)
        {
            var slot = foodSlotInfoArr[i];
            PlayerPrefs.SetInt($"Slot_{i}_X", slot.indexColRow.x);
            PlayerPrefs.SetInt($"Slot_{i}_Y", slot.indexColRow.y);
            PlayerPrefs.SetInt($"Slot_{i}_FoodLevel", slot.foodLevel);
            PlayerPrefs.SetInt($"Slot_{i}_isLock", slot.isLock ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    public void LoadSlotData()
    {
        for (int i = 0; i < foodSlotInfoArr.Length; i++)
        {
            if (PlayerPrefs.HasKey($"Slot_{i}_X")) // 저장된 슬롯이라면
            {
                int x = PlayerPrefs.GetInt($"Slot_{i}_X");
                int y = PlayerPrefs.GetInt($"Slot_{i}_Y");
                int level = PlayerPrefs.GetInt($"Slot_{i}_FoodLevel");
                bool isLock = PlayerPrefs.GetInt($"Slot_{i}_isLock") == 1;

                foodSlotInfoArr[i] = new FoodSlotInfo(x, y)
                {
                    foodLevel = level,
                    isLock = isLock
                };
            }
        }
    }

    private void LoadGoldDiaData()
    {
        if (PlayerPrefs.HasKey("Gold"))
            Gold = PlayerPrefs.GetInt("Gold");

        if (PlayerPrefs.HasKey("Diamond"))
            Diamond = PlayerPrefs.GetInt("Diamond");

        OnPlayerInfoChanged?.Invoke();
    }

    public void UnlockBossStage(int index)
    {
        if (index >= 0 && index < MAX_BOSS_STAGE_COUNT)
        {
            _bossStageOpenStateArr[index] = true;
            SaveBossStageUnlock();
        }
    }

    public void SaveBossStageUnlock()
    {
        for (int i = 0; i < MAX_BOSS_STAGE_COUNT; i++)
        {
            PlayerPrefs.SetInt($"BossSTageOpen_{i}", _bossStageOpenStateArr[i] ? 1 : 0);
        }

        PlayerPrefs.Save();
    }

    public void LoadBossStageUnlock()
    {
        for (int i = 0; i < MAX_BOSS_STAGE_COUNT; i++)
        {
            if (PlayerPrefs.HasKey($"BossSTageOpen_{i}"))
            {
                _bossStageOpenStateArr[i] = PlayerPrefs.GetInt($"BossSTageOpen_{i}") == 1;
            }
            else
            {
                _bossStageOpenStateArr[i] = (i == 0); // 0번 스테이지만 기본 해제
            }
        }
    }

    private void Awake()
    {
        InitPlayerState();

        _bossStageOpenStateArr[0] = true;
        for (int i = 1; i < _bossStageOpenStateArr.Length; i++)
            _bossStageOpenStateArr[i] = false;

        // 모든 강화 레벨을 0으로 초기화
        for (int i = 0; i < (int)PlayerStat.Max; i++)
            _level[(PlayerStat)i] = 0;

        bossStageInfo = Resources.Load<BossStageInfo>("BossStageInfo");

        foodSlotInfoArr[0] = new FoodSlotInfo(2, 1);
        foodSlotInfoArr[1] = new FoodSlotInfo(3, 1);
        foodSlotInfoArr[2] = new FoodSlotInfo(2, 2);
        foodSlotInfoArr[3] = new FoodSlotInfo(3, 2);
        foodSlotInfoArr[4] = new FoodSlotInfo(2, 3);
        foodSlotInfoArr[5] = new FoodSlotInfo(3, 3);

        foodSlotInfoArr[6] = new FoodSlotInfo(1, 1);
        foodSlotInfoArr[7] = new FoodSlotInfo(4, 1);
        foodSlotInfoArr[8] = new FoodSlotInfo(1, 2);
        foodSlotInfoArr[9] = new FoodSlotInfo(4, 2);
        foodSlotInfoArr[10] = new FoodSlotInfo(1, 3);
        foodSlotInfoArr[11] = new FoodSlotInfo(4, 3);

        foodSlotInfoArr[12] = new FoodSlotInfo(0, 1);
        foodSlotInfoArr[13] = new FoodSlotInfo(0, 2);
        foodSlotInfoArr[14] = new FoodSlotInfo(0, 3);
        foodSlotInfoArr[15] = new FoodSlotInfo(2, 0);
        foodSlotInfoArr[16] = new FoodSlotInfo(3, 0);
        foodSlotInfoArr[17] = new FoodSlotInfo(1, 0);
        foodSlotInfoArr[18] = new FoodSlotInfo(4, 0);
        foodSlotInfoArr[19] = new FoodSlotInfo(2, 4);
        foodSlotInfoArr[20] = new FoodSlotInfo(3, 4);
        foodSlotInfoArr[21] = new FoodSlotInfo(1, 4);
        foodSlotInfoArr[22] = new FoodSlotInfo(4, 4);
        foodSlotInfoArr[23] = new FoodSlotInfo(0, 0);
        foodSlotInfoArr[24] = new FoodSlotInfo(0, 4);
        foodSlotInfoArr[25] = new FoodSlotInfo(5, 0);
        foodSlotInfoArr[26] = new FoodSlotInfo(5, 1);
        foodSlotInfoArr[27] = new FoodSlotInfo(5, 2);
        foodSlotInfoArr[28] = new FoodSlotInfo(5, 3);
        foodSlotInfoArr[29] = new FoodSlotInfo(5, 4);
    }

    public FoodSlotInfo GetFoodSlotInfo(int _index)
    {
        int colIndex = _index % 6;
        int rowIndex = _index / 6;
        return foodSlotInfoArr.Where(_p => _p.indexColRow.x == colIndex && _p.indexColRow.y == rowIndex).FirstOrDefault();
    }
    
    protected override void Clear()
    {
        base.Clear();

        //UpdateMoney();
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
    Dictionary<PlayerStat, int> _level = new();


    // 각 스탯(공격력, 체력)의 현재 강화 레벨을 저장하는 자료 구조
    public Dictionary<PlayerStat, int> level { get { return _level; } }

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

    public void LevelUp(PlayerStat stat, float value)
    {
        _stat[stat] += value; // 스탯 수치 증가
        _level[stat]++; // 해당 스탯의 강화 레벨 증가
        OnPlayerStatChanged?.Invoke(); // UI 갱신
    }

    #region Player Info (Gold / Diamond)
    // player info 갱신
    public event Action OnPlayerInfoChanged;

    // 골드 증가 함수
    public void AddGold(int gold)
    {
        Gold += gold;
        OnPlayerInfoChanged?.Invoke();
    }

    // 다이아몬드 증가 함수
    public void AddDiamond(int diamond)
    {
        Diamond += diamond;
        OnPlayerInfoChanged?.Invoke();
    }

    // 골드 감소 함수
    public bool MinusGold(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        OnPlayerInfoChanged?.Invoke();
        return true;
    }

    // 다이아몬드 감소 함수
    public bool MinusDiamond(int amount)
    {
        if (Diamond < amount) return false;
        Diamond -= amount;
        OnPlayerInfoChanged?.Invoke();
        return true;
    }
    #endregion

}
