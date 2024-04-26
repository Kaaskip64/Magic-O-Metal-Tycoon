using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//just a player properties test scripts
public class PlayerPropertiesTest : MonoBehaviour
{
    public static Action<int> MoneyAction;
    [SerializeField]
    private int money;
    public int Money => money;

    private void Awake()
    {
        ShopSystem.MoneyCheck += MoneyCheck;
    }
    private void MoneyChange(int value)
    {
        if (value <= Money)
        {
            money += value;
            MoneyAction?.Invoke(money);
        }

        print("merge test");
    }

    private bool MoneyCheck(ShopProduct product)
    {
        return (Money - product.Price) >= 0;
    }
}
