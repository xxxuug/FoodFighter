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

    [Header("��ȭ ����")]
    public UI_Money uiMoney; // ���/���̾� ����
    public UpgradeInfo[] upgradeInfo; // �ν����Ϳ��� ����

    [Header("��� ����")]
    public LockManager lockManager;

    [Header("������")]
    public Image UpgradeIcon;

    private void Start()
    {
        UpdateUI();
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
        while (Level < 10)
        {
            // var Upgrade = upgradeInfo[Level];
            var Upgrade = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)];

            bool LevelUpSuccess = false;

            switch (Upgrade.moneyType)
            {
                case MoneyType.Gold:
                    LevelUpSuccess = uiMoney.MinusGold(Upgrade.cost);
                    break;
                case MoneyType.Diamond:
                    LevelUpSuccess = uiMoney.MinusDiamond(Upgrade.cost);
                    break;
            }

            if (!LevelUpSuccess)
            {
                Debug.Log("�ڿ��� �����ؼ� ������ �Ұ�");
                yield break;
            }

            Level++;
            // LevelText.text = $"Level {Level.ToString("D2")}";
            Debug.Log($"{Upgrade.name} ��ȭ ����!");

            if (lockManager != null) lockManager.SetAttackLevel(Level);

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

        // �̸� & ���� ǥ��
        NameText.text = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)].name;
        DescriptionText.text = $"{NameText.text}�� <color=red>{Total:F1}</color>�� �����մϴ�.";

        // ��ȭ ���� ��������
        var Upgrade = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)];

        // ������ ����
        if (UpgradeIcon != null && Upgrade.Icon != null)
            UpgradeIcon.sprite = Upgrade.Icon;
    }
}