using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopProduct : MonoBehaviour
{
    private ShopSystem shopSystem; // Reference to the ShopSystem script

    [SerializeField] private string productName = ""; // Name of the product
    [SerializeField] private float price; // Price of the product
    [SerializeField] private int stock; // Stock of the product
    [SerializeField] public GameObject itemPrefab; // Prefab of the item associated with the product

    public string ProductName => productName; // Property to get the product name
    public float Price => price; // Property to get the product price
    public int Stock => stock; // Property to get the product stock

    public enum ProductSubCategory // Enum to define product subcategories
    {
        Default,
        Stage,
        Facilities,
        Constructions,
        Lineups,
        PlayerImports,
    }

    [SerializeField] private ProductSubCategory category = ProductSubCategory.Default; // Category of the product
    public ProductSubCategory Category => category; // Property to get the product category

    private void Awake()
    {
        shopSystem = transform.GetComponentInParent<ShopSystem>(); // Get reference to the parent ShopSystem script
    }

    private void Start()
    {
        shopSystem.AddProduct(this); // Add this product to the ShopSystem
        GetComponent<Button>().onClick.AddListener(Purchase); // Add listener to the button click event
    }

    private void Purchase()
    {
        if (shopSystem != null)
        {
            shopSystem.PurchaseProduct(ProductName); // Start purchase process for this product
        }
    }


    // Method to change the stock of the product
    public void ChangeStock(int value)
    {
        stock += value;
    }

    // Method to change the price of the product
    public void ChangePrice(float price)
    {
        if (price >= 0)
        {
            this.price = price;
        }
    }

    // Method to change the name of the product
    public void ChangeName(string name)
    {
        if (name != null)
        {
            productName = name;
        }
    }

}
