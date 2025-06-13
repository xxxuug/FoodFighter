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

    [Header("���� �׸��� ����, ��ǥ �ο�")]
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] int _hCount = 6;
    [SerializeField] int _vCount = 5;

    private GameObject[,] _slots;

    [Header("���� ��� / �������")]
    [SerializeField] Sprite LockBackground;
    [SerializeField] Sprite LockIcon;
    public Sprite UnlockBackground;

    [Header("���� �� ���� �̹��� ��ȯ")]
    public List<ItemSprite> _spriteLists;

    [Header("���� ���� ��ư")]
    public Button FoodCreateButton;
    public TMP_Text FoodCreateCountText;
    private int _currentCount;
    private int _maxCount = 10;


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
                // FoodSlot�� ��Ʈ�ѷ� �Ѱ��ֱ�
                FoodSlot foodSlot = slot.GetComponent<FoodSlot>();

                // ��� ������ ��� ��ȯ
                Image background = slot.GetComponent<Image>();
                background.sprite = LockBackground;

                // ��� ������ ������ ��ȯ
                Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>();
                icon.sprite = LockIcon;
                icon.color = new Color(0.435f, 0.435f, 0.435f);

                if ((i == 2 || i == 3) && (j >= 1 && j <= 3))
                {
                    background.sprite = UnlockBackground;
                    icon.sprite = null;
                    icon.color = new Color(1f, 1f, 1f, 0f);
                }
            }
        }

        _currentCount = _maxCount;
        SpawnFood(); // ó�� ������ �� ���� ùĭ�� �⺻ ���� ���̰Բ�
        FoodCreateButton.onClick.AddListener(SpawnFood);
    }

    void SpawnFood()
    {
        for (int i = 0; i < _hCount; i++)
        {
            for (int j = 0; j < _vCount; j++)
            {
                GameObject slot = _slots[i, j];
                Image background = slot.GetComponent<Image>();
                Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>(); // Find �Լ� ���� �ʿ�

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

            foreach (var item in _spriteLists)
            {
                if (item.ItemIcon == icon.sprite) // 
                {
                    if (item.ItemLevel > maxLevel) // �� ������ ���� �ִ� �������� ���ٸ�
                    {
                        maxLevel = item.ItemLevel; // �ִ� ������ �� ������ ����
                        maxSprite = icon.sprite; // �����ܵ� �ִ� ���� ���������� ����
                        _foodbullet.SetFoodSprite(maxSprite); // Ǫ��ҷ��� ������ �����ܵ� ����
                    }
                    break;
                }
            }
        }
        Debug.Log("���� �ְ� ���� : " + maxLevel);
        Debug.Log("���� �ְ� ���� ������ : " + maxSprite);
    }
}
