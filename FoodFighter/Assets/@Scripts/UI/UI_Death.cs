using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Death : MonoBehaviour
{
    [Header("플레이어 죽었을 경우")]
    public Image DeathPanel;
    public TMP_Text FailText;
    public TMP_Text PrevStageText;
    private bool _isDead;

    private void Start()
    {
        DeathPanel.gameObject.SetActive(false);
    }
    IEnumerator Fade(float from, float to)
    {
        DeathPanel.gameObject.SetActive(true);

        float time = 0f;
        float duration = 1f;

        Color c = DeathPanel.color;
        Color fc = FailText.color;
        Color pc = PrevStageText.color;

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            float alpha = Mathf.Lerp(from, to, time / duration);

            DeathPanel.color = new Color(c.r, c.g, c.b, alpha);
            FailText.color = new Color(fc.r, fc.g, fc.b, alpha);
            PrevStageText.color = new Color(pc.r, pc.g, pc.b, alpha);

            yield return null;
        }

        DeathPanel.color = new Color(c.r, c.g, c.b, to);
        FailText.color = new Color(fc.r, fc.g, fc.b, to);
        PrevStageText.color = new Color(pc.r, pc.g, pc.b, to);

        if (to == 0) DeathPanel.gameObject.SetActive(false);
    }

    public IEnumerator Death()
    {
        _isDead = true;

        Time.timeScale = 0;
        yield return Fade(0, 1);

        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;

        StageManager.Instance.RemoveAllEnemy();
        StageManager.Instance.SetPrevStage();

        yield return Fade(1, 0);
        _isDead = false;
    }
}
