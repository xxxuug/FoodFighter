using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : UI_Base
{
    [Header("메뉴 버튼")]
    public Button EnhanceButton;
    public Button BossButton;
    public Button DungeonButton;
    public Button ShopButton;
    // 버튼 눌렀을 때 뜨는 강조 패널
    public GameObject ClickPanel;
    public Button CloseButton;

    [Header("각 버튼 기능 패널")]
    public GameObject EnhanceUpgrade;
    public GameObject Boss;
    public GameObject Dungeon;
    public GameObject Shop;

    [Header("각 애니메이션 스크립트")]
    public Menu_OpenAndClose EnhanceUpgradeAnim;
    public Menu_OpenAndClose BossAnim;
    public Menu_OpenAndClose DungeonAnim;
    public Menu_OpenAndClose ShopAnim;

    public Menu_OpenAndClose OpenPanel = null;


    private void Start()
    {
        // 강조 패널 비활성화
        ClickPanel.SetActive(false);
        // 강화 기능 패널 비활성화
        EnhanceUpgrade.SetActive(false);
        // 보스 기능 패널 비활성화
        Boss.SetActive(false);
        // 던전 기능 패널 비활성화
        Dungeon.SetActive(false);
        // 상점 기능 패널 비활성화
        Shop.SetActive(false);

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

    // 강화 버튼 클릭
    void OnClickEnhanceButton()
    {

        if (OpenPanel != null) OpenPanel.ClosePanel();

        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = EnhanceButton.GetComponent<RectTransform>().anchoredPosition;
        // 강화 패널 열기
        EnhanceUpgrade.SetActive(true);

        EnhanceUpgradeAnim.OpenPanel();
        OpenPanel = EnhanceUpgradeAnim;
    }

    // 보스 버튼 클릭
    void OnClickBossButton()
    {
        if (OpenPanel != null) OpenPanel.ClosePanel();

        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = BossButton.GetComponent<RectTransform>().anchoredPosition;
        // 보스 패널 열기
        Boss.SetActive(true);

        BossAnim.OpenPanel();
        OpenPanel = BossAnim;
    }

    // 던전 버튼 클릭
    void OnClickDungeonButton()
    {
        if (OpenPanel != null) OpenPanel.ClosePanel();

        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = DungeonButton.GetComponent<RectTransform>().anchoredPosition;
        // 던전 패널 열기
        Dungeon.SetActive(true);

        DungeonAnim.OpenPanel();
        OpenPanel = DungeonAnim;
    }

    // 상점 버튼 클릭
    void OnClickShopButton()
    {
        if (OpenPanel != null) OpenPanel.ClosePanel();

        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = ShopButton.GetComponent<RectTransform>().anchoredPosition;
        // 상점 패널 열기
        Shop.SetActive(true);

        ShopAnim.OpenPanel();
        OpenPanel = ShopAnim;
    }

    void OnClickCloseButton()
    {
        ClickPanel.SetActive(false);

        if (OpenPanel != null)
        {
            OpenPanel.ClosePanel();
            OpenPanel = null;
        }

        //if (OpenPanel != null)
        //{
        //    OpenPanel.ClosePanel();
        //    OpenPanel = null;   
        //}

        //if (EnhanceUpgrade.activeSelf)
        //{
        //    EnhanceUpgrade.SetActive(false);
        //}
        //if (Boss.activeSelf)
        //{
        //    Boss.SetActive(false);
        //}
        //if (Dungeon.activeSelf)
        //{
        //    Dungeon.SetActive(false);
        //}
        //if (Shop.activeSelf)
        //{
        //    Shop.SetActive(false);
        //}
    }
}
