using EnumDef;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultPopup : MonoBehaviour
{
    [Header("골드 보상")]
    [SerializeField] private GameObject golGroup;
    [SerializeField] private TMP_Text goldText;

    [Header("다이아 보상")]
    [SerializeField] private GameObject diaGroup;
    [SerializeField] private TMP_Text diaText;

    public void SetReward(int gold, int dia)
    {
        bool hasGold = gold > 0;
        bool hasDia = dia > 0;

        golGroup.SetActive(hasGold);
        diaGroup.SetActive(hasDia);

        if (hasGold) goldText.text = gold.ToString();
        if (hasDia) diaText.text = dia.ToString();
    }

    // 승리 후 또는 패배 후 일반 스테이지로 복귀
    public void OnClickEndGame()
    {
        StopAllCoroutines();
        StageManager.Instance.Player.StartCoroutine(StageManager.Instance.Player.ResetDeath());

        SceneManager.LoadScene(Define.GameScene);
    }

    // 승리 후 다음 스테이지 진입
    public void OnClickNextSTage()
    {
        GameManager.Instance.CurBossStageIndex++;
        SceneManager.LoadScene(Define.BossStageScene);

        GameManager.Instance[PlayerStat.CurrentHp] = GameManager.Instance[PlayerStat.MaxHp];
    }

    // 패배 후 다시 재도전
    public void OnClickRetry()
    {
        // GameManager.Instance.CurBossStageIndex = GameManager.Instance.CurBossStageIndex;
        //  SceneManager.LoadScene(Define.BossStageScene);
        //  StageManager.Instance.boss._rewardPopup.SetActive(false);

        Time.timeScale = 1f;

        // 유니티 현재 씬 다시 불러오기
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        GameManager.Instance[PlayerStat.CurrentHp] = GameManager.Instance[PlayerStat.MaxHp];
    }
}
