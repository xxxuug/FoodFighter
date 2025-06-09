using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    public Button GameStartButton;

    void Start()
    {
        GameStartButton.onClick.AddListener(OnClickGameStartButton);
    }

    void OnClickGameStartButton()
    {
        SceneManager.LoadScene(Define.GameScene);
    }
}
