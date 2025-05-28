using UnityEngine;
using UnityEngine.UI;

public class UI_Status : MonoBehaviour
{
    [Header("Status")]
    public Image HpBar;

    private void OnEnable()
    {
        GameManager.Instance.OnPlayerStatChanged += UpdateHpUI;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnPlayerStatChanged -= UpdateHpUI;
    }

    void UpdateHpUI()
    {
        float currentHp = GameManager.Instance[PlayerStat.CurrentHp];
        float maxHp = GameManager.Instance[PlayerStat.MaxHp];
        HpBar.fillAmount = currentHp / maxHp;
    }
}
