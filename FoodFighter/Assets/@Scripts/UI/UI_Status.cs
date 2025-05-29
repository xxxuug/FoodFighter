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

    private void Start()
    {
        GameManager.Instance.OnPlayerStatChanged += UpdateHpUI;
        StageManager.Instance.OnStageInfoChanged += UpdateStageUI;

        // 시작할 때 실행해주기
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
    }

    void UpdateStageUI()
    {
        if (GameManager.Instance == null) return;

        StageText.text = StageManager.Instance.StageInfo.GetDisplayStage();
    }
}
