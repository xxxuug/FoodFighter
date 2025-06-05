using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BossStage : MonoBehaviour
{
    public Button[] BossButtons;

    private void Start()
    {
        for (int i = 0; i < BossButtons.Length; i++)
        {
            // int Stage = i;
            // BossButton[i].onClick.AddListener(() => OnClickBossButton(Stage));
            BossButtons[i].onClick.AddListener(OnClickBossButton);
        }
    }

    void OnClickBossButton()
    {
        SceneManager.LoadScene(1);
    }
}