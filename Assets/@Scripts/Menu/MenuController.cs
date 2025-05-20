using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public GameObject Menu;
    public Button MenuButton;

    private void Start()
    {
        if (Menu != null) Menu.SetActive(false);

        MenuButton.onClick.AddListener(MenuButtonClick);
    }

    void MenuButtonClick()
    {
        // 메뉴가 비활성화 되어있다면
        if (Menu.gameObject.activeSelf == false)
        {
            Menu.SetActive(true); // 활성화 시켜주어라
        }

        // 메뉴가 활성화 되어있다면
        else if (Menu.gameObject.activeSelf == true)
        {
            Menu.gameObject.SetActive(false); // 비활성화 시켜주어라
        }
    }
}
