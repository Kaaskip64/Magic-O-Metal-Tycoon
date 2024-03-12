using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPropertiesTest : MonoBehaviour
{
    public static Action<int> MoneyAction;
    [SerializeField]
    private int money;
    public int Money => money;

    private void Awake()
    {
        ShopSystem.SuccessfulPurchase += MoneyChange;
        ShopSystem.MoneyCheck += MoneyCheck;
    }
    private void MoneyChange(int value)
    {
        if (value <= Money)
        {
            money += value;
            MoneyAction?.Invoke(money);
        }
    }

    private bool MoneyCheck(ShopProduct product)
    {
        return (Money - product.Price) >= 0;
    }
}
