using EnumDef;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : Singleton<SlotController>
{
    private FoodBullet _foodbullet;

    [Header("슬롯 그리드 생성, 좌표 부여")]
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] int _hCount = 6;
    [SerializeField] int _vCount = 5;

    private GameObject[,] _slots;

    [Header("슬롯 잠금 / 잠금해제")]
    [SerializeField] Sprite LockBackground;
    public Sprite LockIcon;
    public Sprite UnlockBackground;

    [Header("음식 생성 버튼")]
    public Button FoodCreateButton;
    public TMP_Text FoodCreateCountText;
    public Image CreateCharge;
    private int _currentCount;
    private int _maxCount = 10;

    [Header("다른 스크립트 외부 참조용 최대 레벨")]
    public int MaxLevelRef;

    protected override void Initialize() { }

    private void Awake()
    {
        MaxLevelRef = 1;
    }

    void Start()
    {
        _slots = new GameObject[_hCount, _vCount];

        for (int i = 0; i < _hCount; i++)
        {
            for (int j = 0; j < _vCount; j++)
            {
                GameObject slot = Instantiate(_slotPrefab, transform);

                var foodSlot = slot.GetComponent<FoodSlot>();
                foodSlot.kIndex = (j * _hCount) + i;

                _slots[i, j] = slot;
            }
        }

        UpdateSlotUnlock();

        _currentCount = _maxCount;

        if (GameManager.Instance.isFirstFoodSpawn == false)
        {
            GameManager.Instance.isFirstFoodSpawn = true;
            SpawnFood();
        }

        FoodCreateButton.onClick.AddListener(SpawnFood);
    }

    public void UpdateSlotUnlock()
    {
        int unlockCount = Mathf.FloorToInt(GameManager.Instance[PlayerStat.SlotCount]);

        var foodSlots = GameManager.Instance.foodSlotInfoArr;
        for (int index = 0; index < foodSlots.Length; index++)
        {
            var foodSlot = foodSlots[index];

            Vector2Int pos = foodSlot.indexColRow;
            int i = pos.x;
            int j = pos.y;

            if (i < 0 || i >= _hCount || j < 0 || j >= _vCount) continue;

            GameObject slot = _slots[i, j];
            Image background = slot.GetComponent<Image>();
            Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>();

            if (index < unlockCount)
            {
                foodSlot.isLock = false;

                background.sprite = UnlockBackground;
                icon.sprite = null;
                icon.color = new Color(1f, 1f, 1f, 0f);

                foreach (var item in FoodData.Instance.FoodLists)
                {
                    if (item.Level == foodSlot.foodLevel) // 아이템레벨이 다음 레벨이라면
                    {
                        icon.sprite = item.Icon; // 해당 레벨 아이콘으로 바꿔주기
                        icon.color = new Color(1f, 1f, 1f, 1f);
                    }
                }
            }
            else
            {
                foodSlot.isLock = true;

                background.sprite = LockBackground;
                icon.sprite = LockIcon;
                icon.color = new Color(0.435f, 0.435f, 0.435f);
            }
        }
    }

    void SpawnFood()
    {
        if (_currentCount > 0)
        {
            for (int i = 0; i < GameManager.Instance.foodSlotInfoArr.Length; i++)
            {
                var foodSlot = GameManager.Instance.foodSlotInfoArr[i];

                if (foodSlot.isLock == false && foodSlot.foodLevel == 0)
                {
                    foodSlot.foodLevel = 1;

                    GameObject slot = _slots[foodSlot.indexColRow.x, foodSlot.indexColRow.y];
                    Image background = slot.GetComponent<Image>();
                    Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>(); // Find 함수 개선 필요

                    icon.color = new Color(1f, 1f, 1f, 1f);
                    icon.sprite = FoodData.Instance.GetFood(1).Icon; // 1레벨 아이콘 가져오기
                    _currentCount--;
                    FoodCreateCountText.text = $"{_currentCount}/{_maxCount}";
                    return;
                }
            }
        }
        else
            Debug.Log("제작할 수 있는 음식 수가 부족합니다.");
    }

    // 총알을 쏘면 함수가 호출되나 보스전에서 총을 쏘지 않으면 호출 x
    // 그래서 보스전에서 총알을 쏘기전에 음식을 합성하면 에러남

    // FoodBullet 찾아오는 함수
    public void FindFoodBullet(FoodBullet bullet)
    {
        _foodbullet = bullet;
        FindMaxFoodBullet();
    }

    // 현재 unlock 슬롯에 존재하는 최고 레벨의 Food 찾는 함수
    public void FindMaxFoodBullet()
    {
        int maxLevel = -1; // 초기 값을 -1로 줘서 아직 찾지 못한 초기 값임을 나타냄
        Sprite maxSprite = null; // 최고 레벨 이미지 null

        foreach (var foodSlot in GameManager.Instance.foodSlotInfoArr)
        {
            if (foodSlot.isLock || foodSlot.foodLevel <= 0) continue;

            if (foodSlot.foodLevel > maxLevel)
            {
                maxLevel = foodSlot.foodLevel;

                var foodinfo = FoodData.Instance.GetFood(maxLevel);
                if (foodSlot != null)
                    maxSprite = foodinfo.Icon;
            }
        }
        if (maxSprite != null)
        {
            _foodbullet?.SetFoodSprite(maxSprite);
            MaxLevelRef = maxLevel;
        }
        //ebug.Log("현재 최고 레벨 : " + maxLevel);
        //Debug.Log("현재 최고 레벨 아이템 : " + maxSprite);
    }
}
