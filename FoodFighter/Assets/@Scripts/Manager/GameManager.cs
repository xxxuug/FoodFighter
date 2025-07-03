using ClassDef;
using EnumDef;
using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameManager : Singleton<GameManager>
{
    const int MAX_BOSS_STAGE_COUNT = 7; // �� ���� ���� Ƚ��
    #region Init

    public int CurBossStageIndex = 0; // ���� ���� ��������

    bool[] _bossStageOpenStateArr = new bool[MAX_BOSS_STAGE_COUNT];
    public bool[] BossStageOpen
    {
        get => _bossStageOpenStateArr;
    }

    //private TMP_Text GoldText;
    //private TMP_Text DiamondText;
    //private TMP_Text TotalAtkText;
    public int Gold { get; set; } = 1000;
    public int Diamond { get; set; } = 50;

    public BossStageInfo bossStageInfo { get; set; }
    public int AttackLevel { get; set; } // ���ݷ� ����

    public FoodSlotInfo[] foodSlotInfoArr = new FoodSlotInfo[6*5];

    public bool isFirstFoodSpawn = false;

    private void Awake()
    {
        InitPlayerState();

        _bossStageOpenStateArr[0] = true;
        for (int i = 1; i < _bossStageOpenStateArr.Length; i++)
            _bossStageOpenStateArr[i] = false;

        //GoldText = GameObject.Find(Define.GoldText)?.GetComponent<TMP_Text>();
        //DiamondText = GameObject.Find(Define.DiamondText)?.GetComponent<TMP_Text>();
        //TotalAtkText = GameObject.Find(Define.TotalAtkText)?.GetComponent<TMP_Text>();

        //OnPlayerInfoChanged += UpdateMoney;
        //UpdateMoney();

        // ��� ��ȭ ������ 0���� �ʱ�ȭ
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

    //private void Start()
    //{
    //    TotalAttack();
    //}

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

    void InitPlayerState() // �÷��̾� ���� �ʱⰪ
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

    // ���� �̸��� Ű, ��ġ�� ������ �����ϴ� ����
    private Dictionary<PlayerStat, float> _stat = new();
    Dictionary<PlayerStat, int> _level = new();


    // �� ����(���ݷ�, ü��)�� ���� ��ȭ ������ �����ϴ� �ڷ� ����
    public Dictionary<PlayerStat, int> level { get { return _level; } }

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

    public void LevelUp(PlayerStat stat, float value)
    {
        _stat[stat] += value; // ���� ��ġ ����
        _level[stat]++; // �ش� ������ ��ȭ ���� ����
        OnPlayerStatChanged?.Invoke(); // UI ����
    }

    #region Player Info (Gold / Diamond)
    // player info ����
    public event Action OnPlayerInfoChanged;
/*
    private PlayerInfo _playerInfo = new PlayerInfo()
    {
        Gold = 1000,
        Diamond = 50,
    };
*/
/*
    public PlayerInfo PlayerInfo
    {
        get { return _playerInfo; }
        set
        {
            _playerInfo = value;
            OnPlayerInfoChanged?.Invoke();
        }
    }
*/
    // ��� ���� �Լ�
    public void AddGold(int gold)
    {
        Gold += gold;
        OnPlayerInfoChanged?.Invoke();
    }

    // ���̾Ƹ�� ���� �Լ�
    public void AddDiamond(int diamond)
    {
        Diamond += diamond;
        OnPlayerInfoChanged?.Invoke();
    }

    // ��� ���� �Լ�
    public bool MinusGold(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        OnPlayerInfoChanged?.Invoke();
        return true;
    }

    // ���̾Ƹ�� ���� �Լ�
    public bool MinusDiamond(int amount)
    {
        if (Diamond < amount) return false;
        Diamond -= amount;
        OnPlayerInfoChanged?.Invoke();
        return true;
    }

    //void UpdateMoney()
    //{
    //    if (GoldText != null)
    //        GoldText.text = Utils.FormatKoreanNumber(_playerInfo.Gold);
    //    if (DiamondText != null)
    //        DiamondText.text = Utils.FormatKoreanNumber(_playerInfo.Diamond);
    //}

    //public float TotalAttack()
    //{
    //    this[PlayerStat.TotalAtk] = this[PlayerStat.Atk] + FoodData.Instance.GetFood(SlotController.Instance.MaxLevelRef).AttackPower;
    //    // �⺻ ũ��Ƽ�� ������(�� ���ݷ�*1.5)���� ������ ũ��Ƽ�� ������ %�� ���� ���� �� ���ݷ¿� ���ϱ�
    //    this[PlayerStat.TotalCriticalDamage] = (this[PlayerStat.TotalAtk] * 1.5f) + ((this[PlayerStat.TotalAtk] * 1.5f) * (this[PlayerStat.CriticalDamage] / 100));

    //    float rand = UnityEngine.Random.Range(0f, 100f); // 0���� 100 ������ ������ ��. �Ҽ��� ����
    //    float damage;

    //    if (rand <= this[PlayerStat.CriticalProbability])
    //    {
    //        // ũ��Ƽ�� ������ ����
    //        damage = this[PlayerStat.TotalCriticalDamage];
    //        Debug.Log($"ũ��Ƽ�� �߻�! ���� ũ��Ƽ�� Ȯ�� : {this[PlayerStat.CriticalProbability]} ������ : {damage}");
    //    }
    //    else
    //    {
    //        // �� ���ݷ����� ������ ����
    //        damage = this[PlayerStat.TotalAtk];
    //    }

    //    TotalAtkText.text = Utils.FormatKoreanNumber((long)this[PlayerStat.TotalAtk]);

    //    return damage;
    //}
    #endregion
}
