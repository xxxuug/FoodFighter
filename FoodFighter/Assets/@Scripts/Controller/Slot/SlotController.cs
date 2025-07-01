using System.Collections.Generic;
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
    private int _currentCount;
    private int _maxCount = 10;

    [Header("다른 스크립트 외부 참조용 최대 레벨")]
    public int MaxLevelRef;

    private List<Vector2Int> _slotUnlockOrder = new List<Vector2Int>()
    {
            new Vector2Int(2, 1),
            new Vector2Int(3, 1),
            new Vector2Int(2, 2),
            new Vector2Int(3, 2),
            new Vector2Int(2, 3),
            new Vector2Int(3, 3),

            new Vector2Int(1, 1),
            new Vector2Int(4, 1),
            new Vector2Int(1, 2),
            new Vector2Int(4, 2),
            new Vector2Int(1, 3),
            new Vector2Int(4, 3),

            new Vector2Int(0, 1),
            new Vector2Int(0, 2),
            new Vector2Int(0, 3),
            new Vector2Int(2, 0),
            new Vector2Int(3, 0),
            new Vector2Int(1, 0),
            new Vector2Int(4, 0),
            new Vector2Int(2, 4),
            new Vector2Int(3, 4),
            new Vector2Int(1, 4),
            new Vector2Int(4, 4),
            new Vector2Int(0, 0),
            new Vector2Int(0, 4),
            new Vector2Int(5, 0),
            new Vector2Int(5, 1),
            new Vector2Int(5, 2),
            new Vector2Int(5, 3),
            new Vector2Int(5, 4),
    };

    protected override void Initialize() 
    {
        DontDestroyOnLoad(this.gameObject);
    }

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
                _slots[i, j] = slot;
            }
        }

        UpdateSlotUnlock();

        _currentCount = _maxCount;
        SpawnFood();
        FoodCreateButton.onClick.AddListener(SpawnFood);
    }

    public void UpdateSlotUnlock()
    {
        int unlockCount = Mathf.FloorToInt(GameManager.Instance[PlayerStat.SlotCount]);

        for (int index = 0; index < _slotUnlockOrder.Count; index++)
        {
            Vector2Int pos = _slotUnlockOrder[index];
            int i = pos.x;
            int j = pos.y;

            if (i < 0 || i >= _hCount || j < 0 || j >= _vCount) continue;

            GameObject slot = _slots[i, j];
            Image background = slot.GetComponent<Image>();
            Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>();

            if (index < unlockCount)
            {
                background.sprite = UnlockBackground;

                // 이미 음식이 들어있다면 건드리지 않도록
                if (icon.sprite == LockIcon)
                {
                    icon.sprite = null;
                    icon.color = new Color(1f, 1f, 1f, 0f);
                }
            }
            else
            {
                background.sprite = LockBackground;

                // 잠금 상태일 때만 아이콘 초기화 
                if (icon.sprite == null)
                {
                    icon.sprite = LockIcon;
                    icon.color = new Color(0.435f, 0.435f, 0.435f);
                }
            }
        }
    }

    void SpawnFood()
    {
        for (int i = 0; i < _hCount; i++)
        {
            for (int j = 0; j < _vCount; j++)
            {
                GameObject slot = _slots[i, j];
                Image background = slot.GetComponent<Image>();
                Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>(); // Find 함수 개선 필요

                if (background.sprite == UnlockBackground && icon.sprite == null)
                {
                    icon.color = new Color(1f, 1f, 1f, 1f);
                    icon.sprite = FoodData.Instance.GetFood(1).Icon; // 1레벨 아이콘 가져오기
                    _currentCount--;
                    FoodCreateCountText.text = $"{_currentCount}/{_maxCount}";
                    return;
                }
            }
        }
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

        foreach (var slot in _slots)
        {
            Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>();

            if (icon.sprite == null) continue;

            foreach (var item in FoodData.Instance.FoodLists)
            {
                if (item.Icon == icon.sprite) // 
                {
                    if (item.Level > maxLevel) // 그 레벨이 현재 최대 레벨보다 높다면
                    {
                        maxLevel = item.Level; // 최대 레벨을 이 레벨로 지정
                        maxSprite = icon.sprite; // 아이콘도 최대 레벨 아이콘으로 지정
                        _foodbullet?.SetFoodSprite(maxSprite); // 푸드불렛의 실질적 아이콘도 변경
                        MaxLevelRef = maxLevel; // 외부 참조용 변수에 최고 레벨 넣어주기

                        //GameManager.Instance.TotalAttack; // 공격력 갱신
                    }
                    break;
                }
            }
        }
        //Debug.Log("현재 최고 레벨 : " + maxLevel);
        //Debug.Log("현재 최고 레벨 아이템 : " + maxSprite);
    }
}
