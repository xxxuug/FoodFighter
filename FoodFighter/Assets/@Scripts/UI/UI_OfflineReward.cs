using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_OfflineReward : MonoBehaviour
{
    public static UI_OfflineReward Instance;

    [SerializeField] private GameObject _popup;
    [SerializeField] private TMP_Text _goldRewardText;
    [SerializeField] private TMP_Text _offlineTimeText;
    [SerializeField] private Button _rewardButton;

    private void Awake()
    {
        Instance = this;
        _popup.SetActive(false); // 처음엔 꺼둠
    }

    public void Show(int reward, System.TimeSpan elapsed)
    {
        _popup.SetActive(true);
        int hours = elapsed.Hours;
        int minutes = elapsed.Minutes;
        int seconds = elapsed.Seconds;

        string timeText = "오프라인 시간: ";

        if (hours > 0)
            timeText += $"{hours}시간 ";
        if (minutes > 0 || hours > 0)
            timeText += $"{minutes}분 ";
       // if (seconds > 0 || (hours == 0 && minutes == 0))
            timeText += $"{seconds}초 ";

        _offlineTimeText.text = timeText.Trim(); // 마지막 공백 제거

        _goldRewardText.text = $"{reward} G";

        _rewardButton.onClick.RemoveAllListeners();
        _rewardButton.onClick.AddListener(() =>
        {
            SoundManager.Instance.PlayClickSound();
            GameManager.Instance.AddGold(reward);
            _popup.SetActive(false);
        });
    }
}