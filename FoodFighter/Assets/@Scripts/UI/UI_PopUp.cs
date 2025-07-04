using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopUp : MonoBehaviour
{
    [SerializeField] GameObject _popUp;
    [SerializeField] TMP_Text _title;
    [SerializeField] TMP_Text _content;
    [SerializeField] Image _icon;
    [SerializeField] Button _transparentPanel;

    private void Start()
    {
        _popUp.SetActive(false);
    }

    public void Open(FoodData.FoodInfo foodInfo)
    {
        _title.text = $"{foodInfo.Level}. {foodInfo.Name}";
        _content.text = $"°ø°Ý·Â {foodInfo.AttackPower}";
        _icon.sprite = foodInfo.Icon;

        _transparentPanel.onClick.AddListener(Close);
        StartCoroutine(AutoClose());
    }

    void Close()
    {
        _popUp.SetActive(false);
    }

    IEnumerator AutoClose()
    {
        yield return new WaitForSeconds(3f);
        _popUp.SetActive(false);
    }
}
