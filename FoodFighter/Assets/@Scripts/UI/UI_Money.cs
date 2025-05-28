using NUnit.Framework.Interfaces;
using TMPro;
using UnityEngine;

public class UI_Money : UI_Base
{
    private int Gold = 1000;
    private int Diamond = 50;

    [Header("Money ≈ÿΩ∫∆Æ")]
    public TMP_Text GoldText;
    public TMP_Text DiamondText;

    private void Start()
    {
        UpdateMoney();
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        UpdateMoney();
    }

    public void AddDiamond(int amount)
    {
        Diamond += amount;
        UpdateMoney();
    }

    public bool MinusGold(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        UpdateMoney();
        return true;
    }

    public bool MinusDiamond(int amount)
    {
        if (Diamond < amount) return false;
        Diamond -= amount;
        UpdateMoney();
        return true;
    }

    void UpdateMoney()
    {
        GoldText.text = $"{Gold}";
        DiamondText.text = $"{Diamond}";
    }
}
