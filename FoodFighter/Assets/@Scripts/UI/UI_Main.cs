using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI_Main : MonoBehaviour
{
    public Button GameStartButton;

    private AudioClip _mainBGM;
    private AudioClip _gameStartSound;

    void Start()
    {
        _mainBGM = Resources.Load<AudioClip>(Define.MainBGMPath);
        _gameStartSound = Resources.Load<AudioClip>(Define.GameStartSoundPath);

        SoundManager.Instance.PlayBGM(_mainBGM, 0.3f);
        GameStartButton.onClick.AddListener(OnClickGameStartButton);
    }

    void OnClickGameStartButton()
    {
        SoundManager.Instance.PlayGameStartSound();
        SceneManager.LoadScene(Define.GameScene);
    }
}
