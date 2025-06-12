using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodSlot : BaseController, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private Image _background; // 슬롯 배경
    private Image _icon; // 슬롯 아이콘

    [Header("기존 정보 저장 데이터")]
    private Vector3 _startPos; // 잘못 드래그 했을 때 원상복귀 시킬 위치
    private Vector3 _startScale; // 기존 스케일 
    private Transform _startParent; // 기존 부모 오브젝트
    private Transform _topParent; // 상단으로 이동시킬 부모 오브젝트

    protected override void Initialize() { }

    void Start()
    {
        _background = GetComponent<Image>();

        foreach (var icon in GetComponentsInChildren<Image>())
        {
            if (icon.gameObject.name == Define.SlotIcon)
            {
                _icon = icon;
                break;
            }
        }

        _topParent = _icon.transform.root.Find(Define.SlotBackground);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_background.sprite == SlotController.Instance.UnlockBackground && _icon.sprite != null)
        {
            // 기존 정보 저장
            _startPos = _icon.rectTransform.position; // iocn의 원래 위치 저장
            _startParent = _icon.transform.parent; // icon의 원래 부모 오브젝트 저장
            _startScale = _icon.rectTransform.localScale; // icon의 원래 스케일 저장

            _icon.transform.SetParent(_topParent, false); // icon을 상단 오브젝트로 이동
            _icon.rectTransform.localScale = _startScale * 1.2f;
            _icon.rectTransform.SetAsLastSibling(); // 현재 부모의 자식 오브젝트 중 가장 밑으로 내려서 최상단에 보이게
        }
        else
            Debug.Log("잠금 상태 슬롯 클릭");
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_icon.sprite != null)
            _icon.rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_icon.sprite != null)
        { 
            _icon.rectTransform.SetParent(_startParent, false); // icon 원래 부모 오브젝트 복구

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            foreach (var result in results)
            {
                var targetSlot = result.gameObject.GetComponent<FoodSlot>();
                if (targetSlot != null && targetSlot != this) // 다른 슬롯이라면
                {
                    if (targetSlot._icon != null && targetSlot._icon.sprite == _icon.sprite) // 아이콘이 같다면
                    {
                        int currentLevel = 0;

                        // 다음 레벨 이미지로 교체
                        foreach (var item in SlotController.Instance._spriteLists)
                        {
                            if (item.ItemIcon == targetSlot._icon.sprite) // 아이콘 같은지 확인하고
                            {
                                currentLevel = item.ItemLevel; // 같으면 해당 아이콘 아이템 레벨을 넣어주기
                                break;
                            }
                        }

                        int nextLevel = currentLevel + 1; // 다음 레벨은 현재 레벨 +1

                        // 복구
                        _icon.rectTransform.position = _startPos; // icon 원래 위치 복구
                        _icon.rectTransform.localScale = _startScale; // icon 원래 스케일 복구
                        _icon.sprite = null;
                        _icon.color = new Color(1f, 1f, 1f, 0f);

                        foreach (var item in SlotController.Instance._spriteLists)
                        {
                            if (item.ItemLevel == nextLevel) // 아이템레벨이 다음 레벨이라면
                            {
                                targetSlot._icon.sprite = item.ItemIcon; // 해당 레벨 아이콘으로 바꿔주기
                                SlotController.Instance.FindMaxFoodBullet(nextLevel); // 바꿔줄 때마다 여기서 현재 존재하는 가장 최고 레벨 아이콘이 뭔지 찾기
                            }
                        }
                        return;
                    }
                }
            }

            _icon.rectTransform.position = _startPos; // icon 원래 위치 복구
            _icon.rectTransform.localScale = _startScale; // icon 원래 스케일 복구
        }
    }
}
