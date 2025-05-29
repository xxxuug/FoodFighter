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
    public UpgradeInfo[] upgradeInfo; // �ν����Ϳ��� ����

    [Header("��� ����")]
    public LockManager[] lockManager;

    [Header("������")]
    public Image UpgradeIcon;

    private int MAX_Level = 10;

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
        while (Level < MAX_Level)
        {
            // var Upgrade = upgradeInfo[Level];
            var Upgrade = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)];

            int CurrentCost = GetCurrentCost(Level, Upgrade);
          //  UpgradeCostText.text = $"{CurrentCost} {Upgrade.moneyType}";

            bool LevelUpSuccess = false;

            switch (Upgrade.moneyType)
            {
                case MoneyType.Gold:
                   // LevelUpSuccess = uiMoney.MinusGold(Upgrade.cost);
                    LevelUpSuccess = GameManager.Instance.MinusGold(CurrentCost);
                    break;
                case MoneyType.Diamond:
                    LevelUpSuccess = GameManager.Instance.MinusDiamond(CurrentCost);
                    break;
            }

            if (!LevelUpSuccess)
            {
                Debug.Log("�ڿ��� �����ؼ� ������ �Ұ�");
                yield break;
            }

            Level++;
            InitUpgradeCostText(); // ��� �ؽ�Ʈ

            // �ش� ���� ��ȭ ����
            float increase = Upgrade.IncreaseNum;
            switch(Upgrade.stateType)
            {
                case PlayerStat.Atk:
                    //GameManager.Instance[this, PlayerStat.Atk] += increase;
                    GameManager.Instance[PlayerStat.Atk] += increase;
                    break;
                case PlayerStat.MaxHp:
                    GameManager.Instance[PlayerStat.CurrentHp] += increase;
                    break;

                    // �� �� ����
            }

            // LevelText.text = $"Level {Level.ToString("D2")}";
            Debug.Log($"{Upgrade.name} ��ȭ ����!");

            //  if (lockManager != null) lockManager.SetAttackLevel(Level);
            if (lockManager != null && lockManager.Length > 0)
            {
                foreach (var lockManagers in lockManager)
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

        float IncreaseLevel = upgradeInfo[0].IncreaseNum;
        float Total = (Level - 1) * IncreaseLevel;
        Total = Mathf.Max(0f, Total); // ������ 0����

        for (int i = 0; i < Level && i < upgradeInfo.Length; i++)
        {
            Total += upgradeInfo[i].IncreaseNum;
        }

        // ���� ���ݷ�
        Debug.Log($"���� Atk: {GameManager.Instance[PlayerStat.Atk]}");
        // ���� ü��
        Debug.Log($"���� MAxHP: {GameManager.Instance[PlayerStat.CurrentHp]}");

        // �̸� & ���� ǥ��
        NameText.text = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)].name;
        DescriptionText.text = $"{NameText.text}�� <color=red>{Total:F1}</color>�� �����մϴ�.";

        // ��ȭ ���� ��������
        var Upgrade = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)];

        // ������ ����
        if (UpgradeIcon != null && Upgrade.Icon != null)
            UpgradeIcon.sprite = Upgrade.Icon;
    }
    
    // ��ȭ ��� ����
    private int GetCurrentCost(int Level, UpgradeInfo upgrade)
    {
        // upgrade.cost: �⺻ ���
        // Pow(): ����
        // upgrade.Increasecost: ��� ������
        // Mathf.FloorToInt(x): �Ҽ��� ������ ������
        return Mathf.FloorToInt(upgrade.cost * Mathf.Pow(upgrade.Increasecost, Level));
    }

    // ��� �ؽ�Ʈ
    void InitUpgradeCostText()
    {
        if (Level >= MAX_Level)
        {
            UpgradeCostText.text = "�ִ� ����";
            return;
        }

        var upgrade = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)];
        int cost = GetCurrentCost(Level, upgrade);
        UpgradeCostText.text = $"{cost} {upgrade.moneyType}";
    }
}