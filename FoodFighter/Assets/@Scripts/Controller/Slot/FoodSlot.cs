using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FoodSlot : BaseController, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    private SlotController _slotController; // SlotController ����

    private Image _background; // ���� ���
    private Image _icon; // ���� ������

    [Header("���� ���� ���� ������")]
    private Vector3 _startPos; // �߸� �巡�� ���� �� ���󺹱� ��ų ��ġ
    private Vector3 _startScale; // ���� ������ 
    private Transform _startParent; // ���� �θ� ������Ʈ
    private Transform _topParent; // ������� �̵���ų �θ� ������Ʈ
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
            // ���� ���� ����
            _startPos = _icon.rectTransform.position; // iocn�� ���� ��ġ ����
            _startParent = _icon.transform.parent; // icon�� ���� �θ� ������Ʈ ����
            _startScale = _icon.rectTransform.localScale; // icon�� ���� ������ ����

            _icon.transform.SetParent(_topParent, false); // icon�� ��� ������Ʈ�� �̵�
            _icon.rectTransform.localScale = _startScale * 1.2f;
            _icon.rectTransform.SetAsLastSibling(); // ���� �θ��� �ڽ� ������Ʈ �� ���� ������ ������ �ֻ�ܿ� ���̰�
        }
        else
            Debug.Log("��� ���� ���� Ŭ��");
    }

    public void OnDrag(PointerEventData eventData)
    {
        _icon.rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _icon.rectTransform.SetParent(_startParent, false); // icon ���� �θ� ������Ʈ ����

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            var targetSlot = result.gameObject.GetComponent<FoodSlot>();
            if (targetSlot != null && targetSlot != this) // �ٸ� �����̶��
            {
                if (targetSlot._icon != null && targetSlot._icon.sprite == _icon.sprite) // �������� ���ٸ�
                {
                    _currentLevel++;
                    Debug.Log("���� ������ ��ȯ");
                    _icon.rectTransform.position = _startPos; // icon ���� ��ġ ����
                    _icon.rectTransform.localScale = _startScale; // icon ���� ������ ����
                    _icon.sprite = null;
                    return;
                }
            }
        }

        _icon.rectTransform.position = _startPos; // icon ���� ��ġ ����
        _icon.rectTransform.localScale = _startScale; // icon ���� ������ ����
    }
}
