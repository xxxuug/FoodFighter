using EnumDef;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Status : MonoBehaviour
{
    [Header("Status")]
    public Image HpBar;
    public TMP_Text HpText;

    [Header("Stage")]
    public TMP_Text StageText;

    [Header("공격력")]
    public TMP_Text TotalAtkText;

    [Header("보상")]
    public TMP_Text GoldText;
    public TMP_Text DiamondText;

    private void Start()
    {
        GameManager.Instance.OnPlayerStatChanged += UpdateHpUI;
        StageManager.Instance.OnStageInfoChanged += UpdateStageUI;

        // 시작할 때 실행해주기
        UpdateHpUI();
        UpdateStageUI();

        DiamondText.text = Utils.FormatKoreanNumber(GameManager.Instance.Diamond);
        GoldText.text = Utils.FormatKoreanNumber(GameManager.Instance.Gold);
    }

    void UpdateHpUI()
    {
        if (GameManager.Instance == null) return;

        float currentHp = GameManager.Instance[PlayerStat.CurrentHp];
        float maxHp = GameManager.Instance[PlayerStat.MaxHp];

        if (HpBar != null && maxHp > 0)
        {
            HpBar.fillAmount = currentHp / maxHp;
            HpText.text = $"{currentHp}";
        }
        
        if (currentHp <= 0)
        {
            HpText.text = "0";
        }
    }

    public void UpdateStageUI()
    {
        if (GameManager.Instance == null) return;

        if (StageManager.Instance.Player.isBossStage == false && StageManager.Instance.Player.isDungeon == false)
            StageText.text = StageManager.Instance.StageInfo.GetDisplayStage();
        else if (StageManager.Instance.Player.isDungeon == true)
        {
            if (GameManager.Instance.SelectDungeon == "골드광산")
            {
                StageText.text = $"골드 광산 {GameManager.Instance.GoldStageDungeon}";
            }
            else if (GameManager.Instance.SelectDungeon == "다이아광산")
            {
                StageText.text = $"다이아 광산 {GameManager.Instance.DiaStageDungeon}";
            }
        }
        else
            StageText.text = $"BossStage {GameManager.Instance.CurBossStageIndex + 1}";
    }

    private void Update()
    {
        UpdateTotalAttack();

        if (DiamondText != null && GameManager.Instance != null)
            DiamondText.text = Utils.FormatKoreanNumber(GameManager.Instance.Diamond);

        if (GoldText != null && GameManager.Instance != null)
            GoldText.text = Utils.FormatKoreanNumber(GameManager.Instance.Gold);

        UpdateStageUI();
    }

    public bool IsCritical = false;

    float UpdateTotalAttack()
    {
        GameManager.Instance[PlayerStat.TotalAtk] = GameManager.Instance[PlayerStat.Atk] + FoodData.Instance.GetFood(SlotController.Instance.MaxLevelRef).AttackPower;
        // 기본 크리티컬 데미지(총 공격력*1.5)에서 증가한 크리티컬 데미지 %를 곱한 값을 총 공격력에 더하기
        GameManager.Instance[PlayerStat.TotalCriticalDamage] = (GameManager.Instance[PlayerStat.TotalAtk] * 1.5f) + ((GameManager.Instance[PlayerStat.TotalAtk] * 1.5f) * (GameManager.Instance[PlayerStat.CriticalDamage] / 100));

        float rand = UnityEngine.Random.Range(0f, 100f); // 0에서 100 사이의 랜덤한 수. 소수점 포함
        float damage;

        if (rand <= GameManager.Instance[PlayerStat.CriticalProbability])
        {
            // 크리티컬 데미지 적용
            IsCritical = true;
            damage = GameManager.Instance[PlayerStat.TotalCriticalDamage];
            Debug.Log($"크리티컬 발생! 현재 크리티컬 확률 : {GameManager.Instance[PlayerStat.CriticalProbability]} 데미지 : {damage}");
        }
        else
        {
            // 총 공격력으로 데미지 적용
            IsCritical = false;
            damage = GameManager.Instance[PlayerStat.TotalAtk];
        }

        TotalAtkText.text = Utils.FormatKoreanNumber((long)GameManager.Instance[PlayerStat.TotalAtk]);

        return damage;
    }

}
