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
    public TMP_Text UpgradeCostText;

    [Header("강화 관련")]
   // public UI_Money uiMoney; // 골드/다이아 참조
    public UpgradeInfo[] upgradeInfo; // 인스펙터에서 설정

    [Header("잠금 연동")]
    public LockManager[] lockManager;

    [Header("아이콘")]
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
                Debug.Log("자원이 부족해서 레벨업 불가");
                yield break;
            }

            Level++;
            InitUpgradeCostText(); // 비용 텍스트

            // 해당 스탯 강화 적용
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

                    // 그 외 스탯
            }

            // LevelText.text = $"Level {Level.ToString("D2")}";
            Debug.Log($"{Upgrade.name} 강화 성공!");

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

        // 현재 공격력
        Debug.Log($"현재 Atk: {GameManager.Instance[PlayerStat.Atk]}");
        // 현재 체력
        Debug.Log($"현재 MAxHP: {GameManager.Instance[PlayerStat.CurrentHp]}");

        // 이름 & 설명 표시
        NameText.text = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)].name;
        DescriptionText.text = $"{NameText.text}이 <color=red>{Total:F1}</color>배 증가합니다.";

        // 강화 정보 가져오기
        var Upgrade = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)];

        // 아이콘 설정
        if (UpgradeIcon != null && Upgrade.Icon != null)
            UpgradeIcon.sprite = Upgrade.Icon;
    }
    
    // 강화 비용 비율
    private int GetCurrentCost(int Level, UpgradeInfo upgrade)
    {
        // upgrade.cost: 기본 비용
        // Pow(): 제곱
        // upgrade.Increasecost: 비용 증가율
        // Mathf.FloorToInt(x): 소수점 버리고 정수로
        return Mathf.FloorToInt(upgrade.cost * Mathf.Pow(upgrade.Increasecost, Level));
    }

    // 비용 텍스트
    void InitUpgradeCostText()
    {
        if (Level >= MAX_Level)
        {
            UpgradeCostText.text = "최대 레벨";
            return;
        }

        var upgrade = upgradeInfo[Mathf.Min(Level, upgradeInfo.Length - 1)];
        int cost = GetCurrentCost(Level, upgrade);
        UpgradeCostText.text = $"{cost} {upgrade.moneyType}";
    }
}