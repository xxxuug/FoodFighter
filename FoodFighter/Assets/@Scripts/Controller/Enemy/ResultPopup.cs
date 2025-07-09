using EnumDef;
using System.Collections;
using TMPro;
using TMPro.EditorUtilities;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultPopup : MonoBehaviour
{
    [Header("골드 보상")]
    [SerializeField] private GameObject golGroup;
    [SerializeField] private TMP_Text goldText;

    [Header("다이아 보상")]
    [SerializeField] private GameObject diaGroup;
    [SerializeField] private TMP_Text diaText;

    [Header("축하 메시지")]
    [SerializeField] private TMP_Text FinalClearText;
    [SerializeField] private Image FinalClearImage;

    private void Start()
    {
        if (FinalClearText != null)
            FinalClearText.gameObject.SetActive(false);

        if (FinalClearImage != null)
            FinalClearImage.gameObject.SetActive(false);
    }

    public void SetReward(int gold, int dia)
    {
        int curIndex = GameManager.Instance.CurBossStageIndex;
        int maxIndex = GameManager.Instance.BossStageOpen.Length - 1;

        // 마지막 스테이지일 경우 축하 메시지 보여주고 종료
        if (curIndex >= maxIndex)
        {
            // gameObject.SetActive(false);

            if (FinalClearText != null)
                StartCoroutine(ShowFinalClearMessage());

            return;
        }
        bool hasGold = gold > 0;
        bool hasDia = dia > 0;

        golGroup.SetActive(hasGold);
        diaGroup.SetActive(hasDia);

        if (hasGold) goldText.text = gold.ToString();
        if (hasDia) diaText.text = dia.ToString();
    }

    IEnumerator ShowFinalClearMessage()
    {
        Time.timeScale = 0f;

        // 축하 메시지
        if (FinalClearText != null && FinalClearImage != null)
        {
            FinalClearText.gameObject.SetActive(true);
            FinalClearImage.gameObject.SetActive(true);

            FinalClearText.text = " 축하합니다 ! \n\n모든 보스를 \n\n클리어하셨습니다 ! ";
        }

        yield return new WaitForSecondsRealtime(1f);

        Time.timeScale = 1f;
        StageManager.Instance.Player.StartCoroutine(StageManager.Instance.Player.ResetDeath());
        SceneManager.LoadScene(Define.GameScene);
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
