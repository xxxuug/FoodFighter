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
    public UpgradeInfo[] UpgradeInfo; // 인스펙터에서 설정

    [Header("잠금 연동")]
    public LockManager[] LockManager;

    [Header("아이콘")]
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
                Debug.Log("자원이 부족해서 레벨업 불가");
                yield break;
            }

            Level++;
            InitUpgradeCostText(); // 비용 텍스트

            // 해당 스탯 강화 적용
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
                    Debug.Log($"{GameManager.Instance[PlayerStat.SlotCount]} 강화됨");

                    if (SlotController.Instance != null)
                        SlotController.Instance.UpdateSlotUnlock();
                    break;

                    // 그 외 스탯
            }

            // LevelText.text = $"Level {Level.ToString("D2")}";
            Debug.Log($"{upgrade.Name} 강화 성공!");

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

        Debug.Log("최대 강화 레벨 도달");
    }

    void UpdateUI()
    {
        LevelText.text = $"Lv.{Level.ToString("D2")}";

        float increaseLevel = UpgradeInfo[0].IncreaseNum;
        float total = (Level - 1) * increaseLevel;
        total = Mathf.Max(0f, total); // 음수면 0으로

        for (int i = 0; i < Level && i < UpgradeInfo.Length; i++)
        {
            total += UpgradeInfo[i].IncreaseNum;
        }

        // 현재 공격력
        //Debug.Log($"현재 Atk: {GameManager.Instance[PlayerStat.Atk]}");
        // 현재 체력
        //Debug.Log($"현재 MAxHP: {GameManager.Instance[PlayerStat.CurrentHp]}");

        // 이름 & 설명 표시
        NameText.text = UpgradeInfo[Mathf.Min(Level, UpgradeInfo.Length - 1)].Name;
        DescriptionText.text = $"{NameText.text}이 <color=red>{total:F1}</color>배 증가합니다.";

        // 강화 정보 가져오기
        var upgrade = UpgradeInfo[Mathf.Min(Level, UpgradeInfo.Length - 1)];

        // 아이콘 설정
        if (UpgradeIcon != null && upgrade.Icon != null)
            UpgradeIcon.sprite = upgrade.Icon;
    }

    // 강화 비용 비율
    private int GetCurrentCost(int level, UpgradeInfo upgrade)
    {
        // upgrade.cost: 기본 비용
        // Pow(): 제곱
        // upgrade.Increasecost: 비용 증가율
        // Mathf.FloorToInt(x): 소수점 버리고 정수로
        return Mathf.FloorToInt(upgrade.Cost * Mathf.Pow(upgrade.Increasecost, level));
    }

    // 비용 텍스트
    void InitUpgradeCostText()
    {
        if (Level >= MAX_LEVEL || GameManager.Instance[PlayerStat.SlotCount] >= 29)
        {
            UpgradeCostText.text = "최대 레벨";
            return;
        }

        var upgrade = UpgradeInfo[Mathf.Min(Level, UpgradeInfo.Length - 1)];
        int cost = GetCurrentCost(Level, upgrade);
        UpgradeCostText.text = $"{cost} {upgrade.MoneyType}";
    }
}