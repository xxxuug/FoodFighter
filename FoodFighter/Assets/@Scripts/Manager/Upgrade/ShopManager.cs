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

    [Header("Ad Remove Package Purchase Success")]
    private int _purchaseCount;
    public TMP_Text PurchaseSuccess;
    public GameObject AdRemovePackageObject;

    [Header("Pop-Up")]
    public GameObject PopUp; // 팝업창
    public GameObject TotalPrice;
    public TMP_Text PopUpTitleText;
    public TMP_Text GoldPriceText;
    public Button PurchaseButton;
    public Button CancelButton;
    public Button OkButton;

    void Start()
    {
        _purchaseCount = PlayerPrefs.GetInt("AdRemovePurchaseCount", 1);

        PopUp.SetActive(false); // 팝업창 비활성화
        PurchaseSuccess.gameObject.SetActive(false); // 구매 완료 텍스트 비활성화

        // 패키지 아이템
        StarterPackage.onClick.AddListener(OnClickStarterPackage);
        AdRemovePackage.onClick.AddListener(OnClickAdRemovePackage);
        DungeonPackage.onClick.AddListener(OnClickDungeonPackage);

        // 광고 제거 패키지 구매 제한 초과 시 
        if (_purchaseCount == 0)
        {
            PurchaseSuccess.gameObject.SetActive(true); // 구매 완료 텍스트 활성화
            AdRemovePackageObject.SetActive(false); // 광고 제거 패키지 오브젝트 비활성화
        }
    }

    #region 패키지 아이템
    void OnClickStarterPackage()
    {
        PopUp.SetActive(true); // 구매 팝업 활성화
        PurchasePopUp(); // 버튼 활성화/비활성화
        PopUpTitleText.text = "스타터 패키지 를 구매하시겠습니까?";
        GoldPriceText.text = "100,000 G";

        // 기존 리스너 제거
        PurchaseButton.onClick.RemoveAllListeners();
        CancelButton.onClick.RemoveAllListeners();

        // 팝업 버튼
        PurchaseButton.onClick.AddListener(OnClickStarterPackagePurchaseButton);
        CancelButton.onClick.AddListener(OnClickCancelButton);
    }

    void OnClickAdRemovePackage()
    { 
        // 구매 횟수 1번
        if (_purchaseCount == 1)
        {
            PopUp.SetActive(true); // 구매 팝업 활성화
            PurchasePopUp(); // 버튼 활성화/비활성화
            PopUpTitleText.text = "광고 제거 패키지 를 구매하시겠습니까?";
            GoldPriceText.text = "2,000,000 G";

            // 기존 리스너 제거
            PurchaseButton.onClick.RemoveAllListeners();
            CancelButton.onClick.RemoveAllListeners();

            // 팝업 버튼
            PurchaseButton.onClick.AddListener(OnClickAdRemovePackagePurchaseButton);
            CancelButton.onClick.AddListener(OnClickCancelButton);
        }
    }

    void OnClickDungeonPackage()
    {
        PopUp.SetActive(true); // 구매 팝업 활성화
        PurchasePopUp(); // 버튼 활성화/비활성화
        PopUpTitleText.text = "던전 패키지 를 구매하시겠습니까?";
        GoldPriceText.text = "300,000 G";

        // 기존 리스너 제거
        PurchaseButton.onClick.RemoveAllListeners();
        CancelButton.onClick.RemoveAllListeners();

        // 팝업 버튼
        PurchaseButton.onClick.AddListener(OnClickDungeonPackagePurchaseButton);
        CancelButton.onClick.AddListener(OnClickCancelButton);
    }
    #endregion

    #region 팝업 버튼
    // 스타터 패키지 구매 눌렀을 시
    void OnClickStarterPackagePurchaseButton()
    {
        if (GameManager.Instance.Gold < 100000)
        {
            PopUpTitleText.text = "보유 골드가 부족합니다!";
        }
        else
        {
            GameManager.Instance.MinusGold(100000);
            PopUpTitleText.text = "구매가 완료되었습니다.";

            // 패키지 능력치 추가 반영
            GameManager.Instance.AddGold(200000);
            GameManager.Instance.AddDiamond(5000);
        }

        OkPopUp(); // 버튼 활성화/비활성화
        OkButton.onClick.AddListener(OnClickOkButton);
    }

    // 광고 제거 패키지 구매 눌렀을 시
    void OnClickAdRemovePackagePurchaseButton()
    {
        if (GameManager.Instance.Gold < 2000000) // 200만
        {
            PopUpTitleText.text = "보유 골드가 부족합니다!";
        }
        else
        {
            // 더 이상 구매할 수 없게
            _purchaseCount = 0;
            PlayerPrefs.SetInt("AdRemovePurchaseCount", _purchaseCount);
            PlayerPrefs.Save();

            PurchaseSuccess.gameObject.SetActive(true); // 구매 완료 텍스트 활성화
            AdRemovePackageObject.SetActive(false); // 광고 제거 패키지 오브젝트 비활성화

            GameManager.Instance.MinusGold(2000000); // 200만
            PopUpTitleText.text = "구매가 완료되었습니다.";
        }

        OkPopUp(); // 버튼 활성화/비활성화
        OkButton.onClick.AddListener(OnClickOkButton);
    }

    // 던전 패키지 구매 눌렀을 시
    void OnClickDungeonPackagePurchaseButton()
    {
        if (GameManager.Instance.Gold < 300000)
        {
            PopUpTitleText.text = "보유 골드가 부족합니다!";
        }
        else
        {
            GameManager.Instance.MinusGold(300000);
            PopUpTitleText.text = "구매가 완료되었습니다.";
        }

        OkPopUp(); // 버튼 활성화/비활성화
        OkButton.onClick.AddListener(OnClickOkButton);
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

    #region 버튼 활성화/비활성화
    void PurchasePopUp()
    {
        TotalPrice.SetActive(true);
        PurchaseButton.gameObject.SetActive(true); // 구매 버튼 활성화
        CancelButton.gameObject.SetActive(true); // 취소 버튼 활성화
        OkButton.gameObject.SetActive(false); // 확인 버튼 비활성화
    }

    void OkPopUp()
    {
        TotalPrice.SetActive(false);
        PurchaseButton.gameObject.SetActive(false); // 구매 버튼 비활성화
        CancelButton.gameObject.SetActive(false); // 취소 버튼 비활성화
        OkButton.gameObject.SetActive(true); // 확인 버튼 활성화
    }
    #endregion
}
