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
                // FoodSlot에 컨트롤러 넘겨주기
                FoodSlot foodSlot = slot.GetComponent<FoodSlot>();

                // 잠금 슬롯의 배경 변환
                Image background = slot.GetComponent<Image>();
                background.sprite = LockBackground;

                // 잠금 슬롯의 아이콘 변환
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
        SpawnFood(); // 처음 시작할 때 슬롯 첫칸에 기본 음식 놓이게끔
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
                Image icon = slot.transform.Find(Define.SlotIcon).GetComponent<Image>(); // Find 함수 개선 필요

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

            foreach (var item in _spriteLists)
            {
                if (item.ItemIcon == icon.sprite) // 
                {
                    if (item.ItemLevel > maxLevel) // 그 레벨이 현재 최대 레벨보다 높다면
                    {
                        maxLevel = item.ItemLevel; // 최대 레벨을 이 레벨로 지정
                        maxSprite = icon.sprite; // 아이콘도 최대 레벨 아이콘으로 지정
                        _foodbullet.SetFoodSprite(maxSprite); // 푸드불렛의 실질적 아이콘도 변경
                    }
                    break;
                }
            }
        }
        Debug.Log("현재 최고 레벨 : " + maxLevel);
        Debug.Log("현재 최고 레벨 아이템 : " + maxSprite);
    }
}
