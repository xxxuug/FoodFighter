using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : MonoBehaviour
{
    [Header("Status")]
    public Image HpBar;
    public TMP_Text HpText;

    [Header("Stage")]
    public TMP_Text MainStageText;
    public TMP_Text SubStageText;

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerStatChanged += UpdateHpUI;
        StageManager.Instance.OnStageInfoChanged += UpdateStageUI;
    }

    private void OnDisable()
    {
        if (Singleton<GameManager>.IsInstance) // 인스턴스가 살아있을 때만 실행되도록
        {
            GameManager.Instance.OnPlayerStatChanged -= UpdateHpUI;
            StageManager.Instance.OnStageInfoChanged -= UpdateStageUI;
        }
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

        MainStageText.text = $"{StageManager.Instance.StageInfo.MainStage} - ";
        SubStageText.text = $"{StageManager.Instance.StageInfo.SubStage}";
    }
}
