using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UI_OfflineReward : MonoBehaviour
{
    public static UI_OfflineReward Instance;

    [SerializeField] private GameObject _popup;
    [SerializeField] private TMP_Text _goldRewardText;
    [SerializeField] private Button _rewardButton;

    private void Awake()
    {
        Instance = this;
        _popup.SetActive(false); // Ã³À½¿£ ²¨µÒ
    }

    public void Show(int reward)
    {
        _popup.SetActive(true);
        _goldRewardText.text = $"{reward} G";

        _rewardButton.onClick.RemoveAllListeners();
        _rewardButton.onClick.AddListener(() =>
        {
            GameManager.Instance.AddGold(reward);
            _popup.SetActive(false);
        });
    }
}