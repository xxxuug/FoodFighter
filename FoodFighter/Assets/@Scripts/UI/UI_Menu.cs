using UnityEngine;
using UnityEngine.UI;

public class UI_Menu : UI_Base
{
    [Header("�޴� ��ư")]
    public Button EnhanceButton;
    public Button BossButton;
    public Button DungeonButton;
    public Button ShopButton;
    // ��ư ������ �� �ߴ� ���� �г�
    public GameObject ClickPanel;
    public Button CloseButton;

    [Header("�� ��ư ��� �г�")]
    public GameObject EnhanceUpgrade;
    public GameObject Boss;
    public GameObject Dungeon;
    public GameObject Shop;

    [Header("�� �ִϸ��̼� ��ũ��Ʈ")]
    public Menu_OpenAndClose EnhanceUpgradeAnim;
    public Menu_OpenAndClose BossAnim;
    public Menu_OpenAndClose DungeonAnim;
    public Menu_OpenAndClose ShopAnim;

    public Menu_OpenAndClose OpenPanel = null;


    private void Start()
    {
        // ���� �г� ��Ȱ��ȭ
        ClickPanel.SetActive(false);
        // ��ȭ ��� �г� ��Ȱ��ȭ
        EnhanceUpgrade.SetActive(false);
        // ���� ��� �г� ��Ȱ��ȭ
        Boss.SetActive(false);
        // ���� ��� �г� ��Ȱ��ȭ
        Dungeon.SetActive(false);
        // ���� ��� �г� ��Ȱ��ȭ
        Shop.SetActive(false);

        // ��ȭ��ư Ŭ������ ��
        EnhanceButton.onClick.AddListener(OnClickEnhanceButton);
        // ������ư Ŭ������ ��
        BossButton.onClick.AddListener(OnClickBossButton);
        // ������ư Ŭ������ ��
        DungeonButton.onClick.AddListener(OnClickDungeonButton);
        // ������ư Ŭ������ ��
        ShopButton.onClick.AddListener(OnClickShopButton);

        // �ݱ� ��ư Ŭ������ ��
        CloseButton.onClick.AddListener(OnClickCloseButton);
    }

    // ��ȭ ��ư Ŭ��
    void OnClickEnhanceButton()
    {

        if (OpenPanel != null) OpenPanel.ClosePanel();

        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = EnhanceButton.GetComponent<RectTransform>().anchoredPosition;
        // ��ȭ �г� ����
        EnhanceUpgrade.SetActive(true);

        EnhanceUpgradeAnim.OpenPanel();
        OpenPanel = EnhanceUpgradeAnim;
    }

    // ���� ��ư Ŭ��
    void OnClickBossButton()
    {
        if (OpenPanel != null) OpenPanel.ClosePanel();

        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = BossButton.GetComponent<RectTransform>().anchoredPosition;
        // ���� �г� ����
        Boss.SetActive(true);

        BossAnim.OpenPanel();
        OpenPanel = BossAnim;
    }

    // ���� ��ư Ŭ��
    void OnClickDungeonButton()
    {
        if (OpenPanel != null) OpenPanel.ClosePanel();

        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = DungeonButton.GetComponent<RectTransform>().anchoredPosition;
        // ���� �г� ����
        Dungeon.SetActive(true);

        DungeonAnim.OpenPanel();
        OpenPanel = DungeonAnim;
    }

    // ���� ��ư Ŭ��
    void OnClickShopButton()
    {
        if (OpenPanel != null) OpenPanel.ClosePanel();

        ClickPanel.SetActive(true);
        ClickPanel.GetComponent<RectTransform>().anchoredPosition = ShopButton.GetComponent<RectTransform>().anchoredPosition;
        // ���� �г� ����
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
