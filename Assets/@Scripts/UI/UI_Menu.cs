using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : UI_Base
{
    [Header("메뉴 버튼")]
    public Button EnhanceButton;
    public Button BossButton;
    public Button DungeonButton;
    public Button ShopButton;
    // 버튼 눌렀을 때 뜨는 패널
    public GameObject ClickPanel;
    public Button CloseButton;

    private void Start()
    {
        ClickPanel.SetActive(false);

        // 강화버튼 클릭했을 때
        EnhanceButton.onClick.AddListener(OnClickEnhanceButton);
        // 보스버튼 클릭했을 때
        BossButton.onClick.AddListener(OnClickBossButton);
        // 던전버튼 클릭했을 때
        DungeonButton.onClick.AddListener(OnClickDungeonButton);
        // 상점버튼 클릭했을 때
        ShopButton.onClick.AddListener(OnClickShopButton);

        // 닫기 버튼 클릭했을 때
        CloseButton.onClick.AddListener(OnClickCloseButton);
    }

    void OnClickEnhanceButton()
    {
        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = EnhanceButton.GetComponent<RectTransform>().anchoredPosition;
        // 강화 패널 열기
    }

    void OnClickBossButton()
    {
        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = BossButton.GetComponent<RectTransform>().anchoredPosition;
        // 보스 패널 열기
    }

    void OnClickDungeonButton()
    {
        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = DungeonButton.GetComponent<RectTransform>().anchoredPosition;
        // 던전 패널 열기
    }

    void OnClickShopButton()
    {
        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = ShopButton.GetComponent<RectTransform>().anchoredPosition;
        // 상점 패널 열기
    }

    void OnClickCloseButton()
    {
        ClickPanel.SetActive(false);
    }
}
