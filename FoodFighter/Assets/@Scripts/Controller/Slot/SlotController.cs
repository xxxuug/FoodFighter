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
    public Image CreateCharge;
    private int _currentCount;
    private int _maxCount = 10;

    [Header("�ٸ� ��ũ��Ʈ �ܺ� ������ �ִ� ����")]
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
                    Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>(); // Find �Լ� ���� �ʿ�

                    icon.color = new Color(1f, 1f, 1f, 1f);
                    icon.sprite = FoodData.Instance.GetFood(1).Icon; // 1���� ������ ��������
                    _currentCount--;
                    FoodCreateCountText.text = $"{_currentCount}/{_maxCount}";
                    return;
                }
            }
        }
        else
            Debug.Log("������ �� �ִ� ���� ���� �����մϴ�.");
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
        //ebug.Log("���� �ְ� ���� : " + maxLevel);
        //Debug.Log("���� �ְ� ���� ������ : " + maxSprite);
    }
}
