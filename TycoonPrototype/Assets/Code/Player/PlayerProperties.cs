using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerProperties Instance { get; private set; }
    public static Action<float> MoneyAction;


    [SerializeField]
    public float money;
    [SerializeField]
    public float credit;


    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoneyChange(float value)
    {
        if(value>=0)
        {
            money += value;
            MoneyAction?.Invoke(money);
        }
        else if((money + value)<0)
        {
            money += value;
            MoneyAction?.Invoke(money);
        }
    }

    public void ChangeCredit(float value)
    {
        credit += value;
    }

    public bool MoneyCheck(ShopProduct product)
    {
        print((money - product.Price) >= 0);
        return (money - product.Price) >= 0;
    }

    public bool MoneyCheck(float price)
    {
        return (money - price) >= 0;
    }
}
