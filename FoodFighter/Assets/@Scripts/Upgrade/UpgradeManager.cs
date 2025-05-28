using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    private int Level = 0;

    [Header("강화 텍스트")]
    public TMP_Text LevelText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;

    [Header("강화 관련")]
    public UI_Money uiMoney; // 골드/다이아 참조
    public UpgradeInfo[] upgradeInfo; // 인스펙터에서 설정

    [Header("잠금 연동")]
    public LockManager lockManager;

    [Header("아이콘")]
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
                Debug.Log("자원이 부족해서 레벨업 불가");
                yield break;
            }

            Level++;
            // LevelText.text = $"Level {Level.ToString("D2")}";
            Debug.Log($"{Upgrade.name} 강화 성공!");

            if (lockManager != null) lockManager.SetAttackLevel(Level);

            UpdateUI();

            yield return new WaitForSeconds(0.2f);
        }

        Debug.Log("최대 강화 레벨 도달");
    }

    void UpdateUI()
    {
        LevelText.text = $"Lv.{Level.ToString("D2")}";

        float IncreaseLevel = upgradeInfo[0].IncreaseNum;
        float Total = (Level - 1) * IncreaseLevel;
        Total = Mathf.Max(0f, Total); // 음수면 0으로

        for (int i = 0; i < Level && i < upgradeInfo.Length; i++)
        {
            Total += upgradeInfo[i].IncreaseNum;
        }

        // 이름 & 설명 표시
        NameText.text = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)].name;
        DescriptionText.text = $"{NameText.text}이 <color=red>{Total:F1}</color>배 증가합니다.";

        // 강화 정보 가져오기
        var Upgrade = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)];

        // 아이콘 설정
        if (UpgradeIcon != null && Upgrade.Icon != null)
            UpgradeIcon.sprite = Upgrade.Icon;
    }
}