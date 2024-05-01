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
        ShopSystem.MoneyCheck += MoneyCheck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMoney(float value)
    {
        if (value <= money)
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
        return (money - product.Price) >= 0;
    }
}
