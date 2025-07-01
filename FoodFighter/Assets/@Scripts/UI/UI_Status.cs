using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : MonoBehaviour
{
    [Header("Status")]
    public Image HpBar;
    public TMP_Text HpText;

    [Header("Stage")]
    public TMP_Text StageText;

    [Header("���ݷ�")]
    public TMP_Text TotalAtkText;

    [Header("����")]
    public TMP_Text GoldText;
    public TMP_Text DiamondText;

    private void Start()
    {
        GameManager.Instance.OnPlayerStatChanged += UpdateHpUI;
        StageManager.Instance.OnStageInfoChanged += UpdateStageUI;

        // ������ �� �������ֱ�
        UpdateHpUI();
        UpdateStageUI();
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

    void UpdateStageUI()
    {
        if (GameManager.Instance == null) return;

        if (StageManager.Instance.Player.isBossStage == false)
            StageText.text = StageManager.Instance.StageInfo.GetDisplayStage();
        else
            StageText.text = $"BossStage {GameManager.Instance.CurBossStageIndex + 1}";
    }

    private void Update()
    {
        TotalAttack();

        if (DiamondText != null && GameManager.Instance != null)
            DiamondText.text = Utils.FormatKoreanNumber(GameManager.Instance.Diamond);

        if (GoldText != null && GameManager.Instance != null)
            GoldText.text = Utils.FormatKoreanNumber(GameManager.Instance.Gold);
    }

    public float TotalAttack()
    {
        GameManager.Instance[PlayerStat.TotalAtk] = GameManager.Instance[PlayerStat.Atk] + FoodData.Instance.GetFood(SlotController.Instance.MaxLevelRef).AttackPower;
        // �⺻ ũ��Ƽ�� ������(�� ���ݷ�*1.5)���� ������ ũ��Ƽ�� ������ %�� ���� ���� �� ���ݷ¿� ���ϱ�
        GameManager.Instance[PlayerStat.TotalCriticalDamage] = (GameManager.Instance[PlayerStat.TotalAtk] * 1.5f) + ((GameManager.Instance[PlayerStat.TotalAtk] * 1.5f) * (GameManager.Instance[PlayerStat.CriticalDamage] / 100));

        float rand = UnityEngine.Random.Range(0f, 100f); // 0���� 100 ������ ������ ��. �Ҽ��� ����
        float damage;

        if (rand <= GameManager.Instance[PlayerStat.CriticalProbability])
        {
            // ũ��Ƽ�� ������ ����
            damage = GameManager.Instance[PlayerStat.TotalCriticalDamage];
            Debug.Log($"ũ��Ƽ�� �߻�! ���� ũ��Ƽ�� Ȯ�� : {GameManager.Instance[PlayerStat.CriticalProbability]} ������ : {damage}");
        }
        else
        {
            // �� ���ݷ����� ������ ����
            damage = GameManager.Instance[PlayerStat.TotalAtk];
        }

        TotalAtkText.text = Utils.FormatKoreanNumber((long)GameManager.Instance[PlayerStat.TotalAtk]);

        return damage;
    }

}
