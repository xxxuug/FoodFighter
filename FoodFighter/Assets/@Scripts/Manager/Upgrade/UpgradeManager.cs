using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private int Level = 0;

    [Header("��ȭ �ؽ�Ʈ")]
    public TMP_Text LevelText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public TMP_Text UpgradeCostText;

    [Header("��ȭ ����")]
    // public UI_Money uiMoney; // ���/���̾� ����
    public UpgradeInfo[] UpgradeInfo; // �ν����Ϳ��� ����

    [Header("��� ����")]
    public LockManager[] LockManager;

    [Header("������")]
    public Image UpgradeIcon;

    private const int MAX_LEVEL = 30;

    private void Start()
    {
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
        while (Level < MAX_LEVEL && GameManager.Instance[PlayerStat.SlotCount] < 30)
        {
            // var Upgrade = upgradeInfo[Level];
            var upgrade = UpgradeInfo[Mathf.Min(Level, UpgradeInfo.Length - 1)];

            int currentCost = GetCurrentCost(Level, upgrade);
            //  UpgradeCostText.text = $"{CurrentCost} {Upgrade.moneyType}";

            bool levelUpSuccess = false;

            switch (upgrade.MoneyType)
            {
                case MoneyType.Gold:
                    // LevelUpSuccess = uiMoney.MinusGold(Upgrade.cost);
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

            Level++;
            InitUpgradeCostText(); // ��� �ؽ�Ʈ

            // �ش� ���� ��ȭ ����
            float increase = upgrade.IncreaseNum;
            switch (upgrade.StateType)
            {
                case PlayerStat.Atk:
                    //GameManager.Instance[this, PlayerStat.Atk] += increase;
                    GameManager.Instance[PlayerStat.Atk] += increase;
                    break;
                case PlayerStat.MaxHp:
                    GameManager.Instance[PlayerStat.CurrentHp] += increase;
                    break;
                case PlayerStat.SlotCount:
                    GameManager.Instance[PlayerStat.SlotCount] += 1;
                    Debug.Log($"{GameManager.Instance[PlayerStat.SlotCount]} ��ȭ��");

                    if (SlotController.Instance != null)
                        SlotController.Instance.UpdateSlotUnlock();
                    break;

                    // �� �� ����
            }

            // LevelText.text = $"Level {Level.ToString("D2")}";
            Debug.Log($"{upgrade.Name} ��ȭ ����!");

            //  if (lockManager != null) lockManager.SetAttackLevel(Level);
            if (LockManager != null && LockManager.Length > 0)
            {
                foreach (var lockManagers in LockManager)
                {
                    if (lockManagers != null)
                        lockManagers.SetAttackLevel(Level);
                }
            }



            UpdateUI();

            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("�ִ� ��ȭ ���� ����");
    }

    void UpdateUI()
    {
        LevelText.text = $"Lv.{Level.ToString("D2")}";

        float increaseLevel = UpgradeInfo[0].IncreaseNum;
        float total = (Level - 1) * increaseLevel;
        total = Mathf.Max(0f, total); // ������ 0����

        for (int i = 0; i < Level && i < UpgradeInfo.Length; i++)
        {
            total += UpgradeInfo[i].IncreaseNum;
        }

        // ���� ���ݷ�
        //Debug.Log($"���� Atk: {GameManager.Instance[PlayerStat.Atk]}");
        // ���� ü��
        //Debug.Log($"���� MAxHP: {GameManager.Instance[PlayerStat.CurrentHp]}");

        // �̸� & ���� ǥ��
        NameText.text = UpgradeInfo[Mathf.Min(Level, UpgradeInfo.Length - 1)].Name;
        DescriptionText.text = $"{NameText.text}�� <color=red>{total:F1}</color>�� �����մϴ�.";

        // ��ȭ ���� ��������
        var upgrade = UpgradeInfo[Mathf.Min(Level, UpgradeInfo.Length - 1)];

        // ������ ����
        if (UpgradeIcon != null && upgrade.Icon != null)
            UpgradeIcon.sprite = upgrade.Icon;
    }

    // ��ȭ ��� ����
    private int GetCurrentCost(int level, UpgradeInfo upgrade)
    {
        // upgrade.cost: �⺻ ���
        // Pow(): ����
        // upgrade.Increasecost: ��� ������
        // Mathf.FloorToInt(x): �Ҽ��� ������ ������
        return Mathf.FloorToInt(upgrade.Cost * Mathf.Pow(upgrade.Increasecost, level));
    }

    // ��� �ؽ�Ʈ
    void InitUpgradeCostText()
    {
        if (Level >= MAX_LEVEL || GameManager.Instance[PlayerStat.SlotCount] >= 29)
        {
            UpgradeCostText.text = "�ִ� ����";
            return;
        }

        var upgrade = UpgradeInfo[Mathf.Min(Level, UpgradeInfo.Length - 1)];
        int cost = GetCurrentCost(Level, upgrade);
        UpgradeCostText.text = $"{cost} {upgrade.MoneyType}";
    }
}