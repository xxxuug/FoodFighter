using System.Collections.Generic;
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
    [Header("슬롯 그리드 생성, 좌표 부여")]
    [SerializeField] GameObject _slotPrefab;
    [SerializeField] int _hCount = 6;
    [SerializeField] int _vCount = 5;

    private GameObject[,] _slots;

    [Header("슬롯 잠금 / 잠금해제")]
    [SerializeField] Sprite LockBackground;
    [SerializeField] Sprite LockIcon;
    [SerializeField] Sprite UnlockBackground;

    [Header("레벨 별 슬롯 이미지 변환")]
    [SerializeField] private List<ItemSprite> _spriteList;

    void Start()
    {
        _slots = new GameObject[_hCount, _vCount];

        for (int i = 0; i < _hCount; i++)
        {
            for (int j = 0; j < _vCount; j++)
            {
                GameObject slot = Instantiate(_slotPrefab, transform);
                _slots[i, j] = slot;

                // 잠금 슬롯의 배경 변환
                Image background = slot.GetComponent<Image>();
                background.sprite = LockBackground;

                // 잠금 슬롯의 아이콘 변환
                Image icon = slot.transform.Find("SlotIcon").GetComponent<Image>();
                icon.sprite = LockIcon;

                if ((i == 2 || i == 3) && (j >= 1 && j <= 3))
                {
                    background.sprite = UnlockBackground;
                    icon.sprite = null;
                }
            }
        }
    }
}
