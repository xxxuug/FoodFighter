using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Status : MonoBehaviour
{
    [Header("Status")]
    public Image HpBar;
    public TMP_Text HpText;

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerStatChanged += UpdateHpUI;
    }

    private void OnDisable()
    {
        if (Singleton<GameManager>.IsInstance) // 인스턴스가 살아있을 때만 실행되도록
            GameManager.Instance.OnPlayerStatChanged -= UpdateHpUI;
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
}
