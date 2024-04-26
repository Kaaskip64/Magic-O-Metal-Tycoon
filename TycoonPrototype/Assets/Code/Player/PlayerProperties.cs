using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperties : MonoBehaviour
{
    // Start is called before the first frame update
    public static PlayerProperties Instance { get; private set; }

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
        ShopSystem.SuccessfulPurchase += ChangeMoney;
        ShopSystem.MoneyCheck += MoneyCheck;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeMoney(float value)
    {
        money += value;
    }

    public void ChangeCredit(float value)
    {
        credit += value;
    }

    private bool MoneyCheck(ShopProduct product)
    {
        return (money - product.Price) >= 0;
    }
}
