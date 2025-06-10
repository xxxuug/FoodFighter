using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodSlot : BaseController, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private SlotController _slotController; // SlotController 참조

    private Image _background; // 슬롯 배경
    private Image _icon; // 슬롯 아이콘

    [Header("기존 정보 저장 데이터")]
    private Vector3 _startPos; // 잘못 드래그 했을 때 원상복귀 시킬 위치
    private Vector3 _startScale; // 기존 스케일 
    private Transform _startParent; // 기존 부모 오브젝트
    private Transform _topParent; // 상단으로 이동시킬 부모 오브젝트
    private int _currentLevel = 1;

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

    public void SetSlotController(SlotController controller)
    {
        _slotController = controller;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_background.sprite == _slotController.UnlockBackground)
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
        _icon.rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
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
                    _currentLevel++;
                    Debug.Log("다음 레벨로 전환");
                    _icon.rectTransform.position = _startPos; // icon 원래 위치 복구
                    _icon.rectTransform.localScale = _startScale; // icon 원래 스케일 복구
                    _icon.sprite = null;
                    return;
                }
            }
        }

        _icon.rectTransform.position = _startPos; // icon 원래 위치 복구
        _icon.rectTransform.localScale = _startScale; // icon 원래 스케일 복구
    }
}
