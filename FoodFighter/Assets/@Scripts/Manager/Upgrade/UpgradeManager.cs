using System.Collections;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private int _level = 0;

    [Header("��ȭ �ؽ�Ʈ")]
    public TMP_Text LevelText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public TMP_Text UpgradeCostText;
    private string _originDescriptionText; // ������ ���� ���� �ؽ�Ʈ ����

    [Header("��ȭ ����")]
    public UpgradeInfo[] UpgradeInfo; // �ν����Ϳ��� ����

    [Header("��� ����")]
    public LockManager[] LockManager;

    private const int MAX_LEVEL = 1000000;

    private void Start()
    {
        _originDescriptionText = DescriptionText.text; // ���� ���� �ؽ�Ʈ�� ������ ������ ����

        UpdateUI();
        InitUpgradeCostText();
    }

    public void OnPointerDown()
    {
        StartCoroutine(LevelUp());
    }

    public void OnPointerUp()
    {
        StopAllCoroutines();
    }

    IEnumerator LevelUp()
    {
        while (_level < MAX_LEVEL && GameManager.Instance[PlayerStat.SlotCount] < 30)
        {
            var upgrade = UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)];

            int currentCost = GetCurrentCost(_level, upgrade);

            bool levelUpSuccess = false;

            switch (upgrade.MoneyType)
            {
                case MoneyType.Gold:
                    levelUpSuccess = GameManager.Instance.MinusGold(currentCost);
                    break;
                case MoneyType.Diamond:
                    levelUpSuccess = GameManager.Instance.MinusDiamond(currentCost);
                    break;
            }

            if (!levelUpSuccess)
            {
                Debug.Log("�ڿ��� �����ؼ� ������ �Ұ�");
                yield break;
            }

            _level++;
            InitUpgradeCostText(); // ��� �ؽ�Ʈ

            // �ش� ���� ��ȭ ����
            float increase = upgrade.IncreaseNum;
            switch (upgrade.StateType)
            {
                case PlayerStat.Atk:
                    GameManager.Instance[PlayerStat.Atk] += increase;
                    break;
                case PlayerStat.MaxHp:
                    GameManager.Instance[PlayerStat.MaxHp] += increase;
                    break;
                case PlayerStat.CriticalProbability:
                    GameManager.Instance[PlayerStat.CriticalProbability] += increase;
                    break;
                case PlayerStat.SlotCount:
                    GameManager.Instance[PlayerStat.SlotCount] += 1;
                    Debug.Log($"{GameManager.Instance[PlayerStat.SlotCount]} ��ȭ��");

                    if (SlotController.Instance != null)
                        SlotController.Instance.UpdateSlotUnlock();
                    break;

                    // �� �� ����
            }

            Debug.Log($"{upgrade.Name} ��ȭ ����!");

            if (LockManager != null && LockManager.Length > 0)
            {
                foreach (var lockManagers in LockManager)
                {
                    if (lockManagers != null)
                        lockManagers.SetAttackLevel(_level);
                }
            }

            UpdateUI();

            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("�ִ� ��ȭ ���� ����");
    }

    void UpdateUI()
    {
        LevelText.text = $"Lv.{_level.ToString("D2")}";

        float increaseLevel = UpgradeInfo[0].IncreaseNum;
        float total = (_level - 1) * increaseLevel;
        total = Mathf.Max(0f, total); // ������ 0����

        for (int i = 0; i < _level && i < UpgradeInfo.Length; i++)
        {
            total += UpgradeInfo[i].IncreaseNum;
        }

        // ���� ���ݷ�
        //Debug.Log($"���� Atk: {GameManager.Instance[PlayerStat.Atk]}");

        // �̸� & ���� ǥ��
        NameText.text = UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)].Name;
        string totValue = Utils.FormatKoreanNumber((long)total);
        string nextValue = $"<color=green>{Utils.FormatKoreanNumber((long)(total + UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)].IncreaseNum))}</color>";

        DescriptionText.text = _originDescriptionText
            .Replace("n", totValue)
            .Replace("x", nextValue);
    }

    // ��ȭ ��� ����
    private int GetCurrentCost(int level, UpgradeInfo upgrade)
    {
        // upgrade.cost: �⺻ ���
        // Pow(): ����
        // upgrade.Increasecost: ��� ������
        // Mathf.FloorToInt(x): �Ҽ��� ������ ������

        // ����� ��� ����
        if (UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)].MoneyType == MoneyType.Gold)
            return Mathf.FloorToInt(upgrade.Cost * Mathf.Pow(upgrade.IncreaseCost, level));
        // ���̾��� ��� ����
        else
            return Mathf.FloorToInt(upgrade.Cost + upgrade.IncreaseCost * level);
    }

    // ��� �ؽ�Ʈ
    void InitUpgradeCostText()
    {
        if (_level >= MAX_LEVEL || GameManager.Instance[PlayerStat.SlotCount] >= 29)
        {
            UpgradeCostText.text = "�ִ� ����";
            return;
        }

        var upgrade = UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)];
        int cost = GetCurrentCost(_level, upgrade);
        UpgradeCostText.text = $"       {cost}";
    }
}