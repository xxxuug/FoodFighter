using System.Collections;
using TMPro;
using Unity.Collections;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    private int _level = 0;

    [Header("강화 텍스트")]
    public TMP_Text LevelText;
    public TMP_Text NameText;
    public TMP_Text DescriptionText;
    public TMP_Text UpgradeCostText;
    private string _originDescriptionText; // 저장할 원본 설명 텍스트 변수

    [Header("강화 관련")]
    public UpgradeInfo[] UpgradeInfo; // 인스펙터에서 설정

    [Header("잠금 연동")]
    public LockManager[] LockManager;

    private const int MAX_LEVEL = 1000000;

    private void Start()
    {
        _originDescriptionText = DescriptionText.text; // 원본 설명 텍스트를 오리진 변수에 저장

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
                Debug.Log("자원이 부족해서 레벨업 불가");
                yield break;
            }

            _level++;
            InitUpgradeCostText(); // 비용 텍스트

            // 해당 스탯 강화 적용
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
                    Debug.Log($"{GameManager.Instance[PlayerStat.SlotCount]} 강화됨");

                    if (SlotController.Instance != null)
                        SlotController.Instance.UpdateSlotUnlock();
                    break;

                    // 그 외 스탯
            }

            Debug.Log($"{upgrade.Name} 강화 성공!");

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

        Debug.Log("최대 강화 레벨 도달");
    }

    void UpdateUI()
    {
        LevelText.text = $"Lv.{_level.ToString("D2")}";

        float increaseLevel = UpgradeInfo[0].IncreaseNum;
        float total = (_level - 1) * increaseLevel;
        total = Mathf.Max(0f, total); // 음수면 0으로

        for (int i = 0; i < _level && i < UpgradeInfo.Length; i++)
        {
            total += UpgradeInfo[i].IncreaseNum;
        }

        // 현재 공격력
        //Debug.Log($"현재 Atk: {GameManager.Instance[PlayerStat.Atk]}");

        // 이름 & 설명 표시
        NameText.text = UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)].Name;
        string totValue = Utils.FormatKoreanNumber((long)total);
        string nextValue = $"<color=green>{Utils.FormatKoreanNumber((long)(total + UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)].IncreaseNum))}</color>";

        DescriptionText.text = _originDescriptionText
            .Replace("n", totValue)
            .Replace("x", nextValue);
    }

    // 강화 비용 비율
    private int GetCurrentCost(int level, UpgradeInfo upgrade)
    {
        // upgrade.cost: 기본 비용
        // Pow(): 제곱
        // upgrade.Increasecost: 비용 증가율
        // Mathf.FloorToInt(x): 소수점 버리고 정수로

        // 골드일 경우 계산식
        if (UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)].MoneyType == MoneyType.Gold)
            return Mathf.FloorToInt(upgrade.Cost * Mathf.Pow(upgrade.IncreaseCost, level));
        // 다이아일 경우 계산식
        else
            return Mathf.FloorToInt(upgrade.Cost + upgrade.IncreaseCost * level);
    }

    // 비용 텍스트
    void InitUpgradeCostText()
    {
        if (_level >= MAX_LEVEL || GameManager.Instance[PlayerStat.SlotCount] >= 29)
        {
            UpgradeCostText.text = "최대 레벨";
            return;
        }

        var upgrade = UpgradeInfo[Mathf.Min(_level, UpgradeInfo.Length - 1)];
        int cost = GetCurrentCost(_level, upgrade);
        UpgradeCostText.text = $"       {cost}";
    }
}