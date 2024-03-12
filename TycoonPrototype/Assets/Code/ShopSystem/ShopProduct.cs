using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class ShopProduct : MonoBehaviour 
{
    private ShopSystem shopSystem;

    [SerializeField]
    private string productName = "";
    [SerializeField]
    private float price;
    [SerializeField]
    private int stock;
    [SerializeField]
    public string Name => productName;
    public float Price => price;
    public int Stock => stock;

    public GameObject itemPrefab;

    public enum ProductCategory
    {
        Default,
        Stage,
        Facilities,
        Constructions,
        Lineups,
        PlayerImports,
    }
    [SerializeField] private ProductCategory category = ProductCategory.Default;
    public ProductCategory Category => category;

    private void Awake()
    {
        shopSystem = transform.GetComponentInParent<ShopSystem>();
    }

    private void Start()
    {
        shopSystem.AddProduct(this);
        GetComponent<Button>().onClick.AddListener(Purchase);
    }
    public void Purchase()
    {
        if (shopSystem != null)
        {
            shopSystem.PurchaseProduct(Name);
        }
    }

    public void ChangeStock(int value)
    {
        stock += value;
    }

    public void ChangePrice(float price)
    {
        if(price>=0)
        {
            this.price = price;
        }
    }

    public void ChangeName(string name)
    {
        if(name != null)
        {
            productName = name;
        }
    }
    
}
