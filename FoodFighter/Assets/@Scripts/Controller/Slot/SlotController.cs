using EnumDef;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotController : Singleton<SlotController>
{
    private FoodBullet _foodbullet;

    [Header("���� �׸��� ����, ��ǥ �ο�")]
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] int _hCount = 6;
    [SerializeField] int _vCount = 5;

    private GameObject[,] _slots;

    [Header("���� ��� / �������")]
    [SerializeField] Sprite LockBackground;
    public Sprite LockIcon;
    public Sprite UnlockBackground;

    [Header("���� ���� ��ư")]
    public Button FoodCreateButton;
    public TMP_Text FoodCreateCountText;
    private int _currentCount;
    private int _maxCount = 10;

    [Header("�ٸ� ��ũ��Ʈ �ܺ� ������ �ִ� ����")]
    public int MaxLevelRef;
/*
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
*/
    protected override void Initialize() 
    {
        //DontDestroyOnLoad(this.gameObject);
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
                
                var foodSlot = slot.GetComponent<FoodSlot>();
                foodSlot.kIndex = (j* _hCount) + i;
/*
                var tmpText = slot.GetComponentInChildren<TMP_Text>();
                tmpText.text = foodSlot.kIndex.ToString();
*/
                _slots[i, j] = slot;
            }
        }

        UpdateSlotUnlock();

        _currentCount = _maxCount;

        if(GameManager.Instance.isFirstFoodSpawn == false)
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
                    if (item.Level == foodSlot.foodLevel) // �����۷����� ���� �����̶��
                    {
                        icon.sprite = item.Icon; // �ش� ���� ���������� �ٲ��ֱ�
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
        for(int i = 0; i < GameManager.Instance.foodSlotInfoArr.Length; i++)
        {
            var foodSlot = GameManager.Instance.foodSlotInfoArr[i];

            if (foodSlot.isLock == false && foodSlot.foodLevel == 0/*background.sprite == UnlockBackground && icon.sprite == null*/)
            {
                foodSlot.foodLevel = 1;

                GameObject slot = _slots[foodSlot.indexColRow.x, foodSlot.indexColRow.y];
                Image background = slot.GetComponent<Image>();
                Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>(); // Find �Լ� ���� �ʿ�

                icon.color = new Color(1f, 1f, 1f, 1f);
                icon.sprite = FoodData.Instance.GetFood(1).Icon; // 1���� ������ ��������
                _currentCount--;
                FoodCreateCountText.text = $"{_currentCount}/{_maxCount}";
                return;
            }
        }
    }

    // �Ѿ��� ��� �Լ��� ȣ��ǳ� ���������� ���� ���� ������ ȣ�� x
    // �׷��� ���������� �Ѿ��� ������� ������ �ռ��ϸ� ������

    // FoodBullet ã�ƿ��� �Լ�
    public void FindFoodBullet(FoodBullet bullet)
    {
        _foodbullet = bullet;
        FindMaxFoodBullet();
    }

    // ���� unlock ���Կ� �����ϴ� �ְ� ������ Food ã�� �Լ�
    public void FindMaxFoodBullet()
    {
        int maxLevel = -1; // �ʱ� ���� -1�� �༭ ���� ã�� ���� �ʱ� ������ ��Ÿ��
        Sprite maxSprite = null; // �ְ� ���� �̹��� null

        foreach (var slot in _slots)
        {
            Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>();

            if (icon.sprite == null) continue;

            foreach (var item in FoodData.Instance.FoodLists)
            {
                if (item.Icon == icon.sprite) // 
                {
                    if (item.Level > maxLevel) // �� ������ ���� �ִ� �������� ���ٸ�
                    {
                        maxLevel = item.Level; // �ִ� ������ �� ������ ����
                        maxSprite = icon.sprite; // �����ܵ� �ִ� ���� ���������� ����
                        _foodbullet?.SetFoodSprite(maxSprite); // Ǫ��ҷ��� ������ �����ܵ� ����
                        MaxLevelRef = maxLevel; // �ܺ� ������ ������ �ְ� ���� �־��ֱ�

                        //GameManager.Instance.TotalAttack; // ���ݷ� ����
                    }
                    break;
                }
            }
        }
        //Debug.Log("���� �ְ� ���� : " + maxLevel);
        //Debug.Log("���� �ְ� ���� ������ : " + maxSprite);
    }
}
