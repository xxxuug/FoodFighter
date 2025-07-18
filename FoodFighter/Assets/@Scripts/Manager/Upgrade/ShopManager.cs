using EnumDef;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    [Header("PACKAGE ITEM")]
    public Button StarterPackage; // 스타터 패키지 버튼
    public Button AdRemovePackage; // 광고 제거 패키지 버튼
    public Button DungeonPackage; // 던전 패키지 버튼

    [Header("Pop-Up")]
    public GameObject PopUp; // 팝업창
    public TMP_Text PopUpTitleText;
    public TMP_Text GoldPriceText;
    public Button PurchaseButton;
    public Button CancelButton;
    public Button OkButton;

    void Start()
    {
        PopUp.SetActive(false); // 팝업창 비활성화

        // 패키지 아이템
        StarterPackage.onClick.AddListener(OnClickStarterPackage);
        AdRemovePackage.onClick.AddListener(OnClickAdRemovePackage);
        DungeonPackage.onClick.AddListener(OnClickDungeonPackage);
    }

    #region 패키지 아이템
    void OnClickStarterPackage()
    {
        PopUp.SetActive(true); // 구매 팝업 활성화
        OkButton.gameObject.SetActive(false); // 확인 버튼 비활성화
        PopUpTitleText.text = "스타터 패키지 를 구매하시겠습니까?";
        GoldPriceText.text = "1,000 G";

        // 팝업 버튼
        PurchaseButton.onClick.AddListener(OnClickStarterPackagePurchaseButton);
        CancelButton.onClick.AddListener(OnClickCancelButton);
    }

    void OnClickAdRemovePackage()
    {
        PopUp.SetActive(true); // 구매 팝업 활성화
        OkButton.gameObject.SetActive(false); // 확인 버튼 비활성화
        PopUpTitleText.text = "광고 제거 패키지 를 구매하시겠습니까?";
        GoldPriceText.text = "2,000 G";

        // 팝업 버튼
        PurchaseButton.onClick.AddListener(OnClickAdRemovePackagePurchaseButton);
        CancelButton.onClick.AddListener(OnClickCancelButton);
    }

    void OnClickDungeonPackage()
    {
        PopUp.SetActive(true); // 구매 팝업 활성화
        OkButton.gameObject.SetActive(false); // 확인 버튼 비활성화
        PopUpTitleText.text = "던전 패키지 를 구매하시겠습니까?";
        GoldPriceText.text = "3,000 G";

        // 팝업 버튼
        PurchaseButton.onClick.AddListener(OnClickDungeonPackagePurchaseButton);
        CancelButton.onClick.AddListener(OnClickCancelButton);
    }
    #endregion

    #region 팝업 버튼
    // 스타터 패키지 구매 눌렀을 시
    void OnClickStarterPackagePurchaseButton()
    {
        PopUp.SetActive(false); // 구매 팝업 비활성화

        // 적혀있는 골드만큼 차감되고 패키지에 있는 아이템 효과들 적용하는 로직
        GameManager.Instance.MinusGold(1000);

        // 만약 보유 골드 부족 시 똑같이 PopUp 뜨되 purchase랑 cancel은 비활성화 하고 ok 활성화
    }

    // 광고 제거 패키지 구매 눌렀을 시
    void OnClickAdRemovePackagePurchaseButton()
    {
        PopUp.SetActive(false); // 구매 팝업 비활성화

        // 적혀있는 골드만큼 차감되고 패키지에 있는 아이템 효과들 적용하는 로직
        GameManager.Instance.MinusGold(2000);
        // 만약 보유 골드 부족 시 똑같이 PopUp 뜨되 purchase랑 cancel은 비활성화 하고 ok 활성화
    }

    // 던전 패키지 구매 눌렀을 시
    void OnClickDungeonPackagePurchaseButton()
    {
        PopUp.SetActive(false); // 구매 팝업 비활성화

        // 적혀있는 골드만큼 차감되고 패키지에 있는 아이템 효과들 적용하는 로직
        GameManager.Instance.MinusGold(3000);

        // 만약 보유 골드 부족 시 똑같이 PopUp 뜨되 purchase랑 cancel은 비활성화 하고 ok 활성화
        if (GameManager.Instance.Gold < 3000)
        {
            PopUp.SetActive(true); // 팝업 재활성화
            PopUpTitleText.text = "보유 골드가 부족합니다!";

            PurchaseButton.gameObject.SetActive(false); // 구매 버튼 비활성화
            CancelButton.gameObject.SetActive(false); // 취소 버튼 비활성화
            OkButton.gameObject.SetActive(true); // 확인 버튼 활성화

            OkButton.onClick.AddListener(OnClickOkButton);
        }
    }

    void OnClickCancelButton()
    {
        PopUp.SetActive(false); // 구매 팝업 비활성화
    }

    void OnClickOkButton()
    {
        PopUp.SetActive(false); // 구매 팝업 비활성화
    }
    #endregion
}
