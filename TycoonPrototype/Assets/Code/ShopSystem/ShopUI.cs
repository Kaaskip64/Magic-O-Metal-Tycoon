using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text MoneyText;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPropertiesTest.MoneyAction += MoneyTextChange;
    }

    private void MoneyTextChange(int currentMoney)
    {
        MoneyText.text = "Money: " + currentMoney;
    }
}
