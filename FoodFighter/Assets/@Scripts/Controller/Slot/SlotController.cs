using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemSprite
{
    public int ItemLevel;
    public Sprite ItemIcon;
}

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
    [SerializeField] Sprite LockIcon;
    public Sprite UnlockBackground;

    [Header("레벨 별 슬롯 이미지 변환")]
    public List<ItemSprite> _spriteLists;

    [Header("음식 생성 버튼")]
    public Button FoodCreateButton;
    public TMP_Text FoodCreateCountText;
    private int _currentCount;
    private int _maxCount = 10;

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

    protected override void Initialize() { }

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
                icon.sprite = null;
                icon.color = new Color(1f, 1f, 1f, 0f);
            }
            else
            {
                background.sprite = LockBackground;
                icon.sprite = LockIcon;
                icon.color = new Color(0.435f, 0.435f, 0.435f);
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
                Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>();

                if (background.sprite == UnlockBackground && icon.sprite == null)
                {
                    icon.color = new Color(1f, 1f, 1f, 1f);
                    icon.sprite = _spriteLists[0].ItemIcon;
                    _currentCount--;
                    FoodCreateCountText.text = $"{_currentCount}/{_maxCount}";
                    return;
                }
            }
        }
    }

    public void FindFoodBullet(FoodBullet bullet)
    {
        _foodbullet = bullet;
        FindMaxFoodBullet();
    }

    public void FindMaxFoodBullet()
    {
        int maxLevel = -1;
        Sprite maxSprite = null;

        foreach (var slot in _slots)
        {
            Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>();

            if (icon.sprite == null) continue;

            foreach (var item in _spriteLists)
            {
                if (item.ItemIcon == icon.sprite)
                {
                    if (item.ItemLevel > maxLevel)
                    {
                        maxLevel = item.ItemLevel;
                        maxSprite = icon.sprite;
                        _foodbullet.SetFoodSprite(maxSprite);
                    }
                    break;
                }
            }
        }

        // Debug.Log("현재 최고 레벨 : " + maxLevel);
        // Debug.Log("현재 최고 레벨 아이템 : " + maxSprite);
    }
}