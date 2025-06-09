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

public class FoodSlot : MonoBehaviour
{
    [Header("���� �׸��� ����, ��ǥ �ο�")]
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] int _hCount = 6;
    [SerializeField] int _vCount = 5;

    private GameObject[,] _slots;

    [Header("���� ��� / �������")]
    [SerializeField] Sprite LockBackground;
    [SerializeField] Sprite LockIcon;
    [SerializeField] Sprite UnlockBackground;

    [Header("���� �� ���� �̹��� ��ȯ")]
    [SerializeField] private List<ItemSprite> _spriteList;

    [Header("���� ���� ��ư")]
    public Button FoodCreateButton;
    public TMP_Text FoodCreateCountText;
    private int _currentCount;
    private int _maxCount = 10;

    void Start()
    {
        _slots = new GameObject[_hCount, _vCount];

        for (int i = 0; i < _hCount; i++)
        {
            for (int j = 0; j < _vCount; j++)
            {
                GameObject slot = Instantiate(_slotPrefab, transform);
                _slots[i, j] = slot;

                // ��� ������ ��� ��ȯ
                Image background = slot.GetComponent<Image>();
                background.sprite = LockBackground;

                // ��� ������ ������ ��ȯ
                Image icon = slot.transform.Find("SlotIcon").GetComponent<Image>();
                icon.sprite = LockIcon;

                if ((i == 2 || i == 3) && (j >= 1 && j <= 3))
                {
                    background.sprite = UnlockBackground;
                    icon.sprite = null;
                }
            }
        }

        _currentCount = _maxCount;
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
                Image icon = slot.transform.Find("SlotIcon").GetComponent<Image>();

                if (background.sprite == UnlockBackground && icon.sprite == null)
                {
                    icon.sprite = _spriteList[0].ItemIcon;
                    _currentCount--;
                    FoodCreateCountText.text = $"{_maxCount}/{_currentCount}";
                    return;
                }
            }
        }
    }
}
