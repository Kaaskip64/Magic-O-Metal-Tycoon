using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopSystem : MonoBehaviour
{
    public static Func<ShopProduct, bool> MoneyCheck; // Function to check if player has enough money

    public GameObject upperBackground;

    private Dictionary<string, ShopProduct> productDict = new Dictionary<string, ShopProduct>(); // Dictionary to store products

    private ShopUI shopUI; // Reference to the ShopUI script

    private void Start()
    {
        shopUI = GetComponent<ShopUI>(); // Get reference to ShopUI component
    }

    // Method to handle purchasing a product
    public void PurchaseProduct(string productName)
    {
        if (productDict.ContainsKey(productName)) // Check if the product exists
        {
            ShopProduct product = productDict[productName]; // Get the product

            if (PlayerProperties.Instance.MoneyCheckThenChange(product)) // Check if player has enough money
            {
                BuildingSystem.currentInstance.InitializeWithBuilding(product); // Initialize with building
                Debug.Log("Purchase product: " + productName);
                upperBackground.SetActive(false);
            }
            else
            {
                Debug.Log("Insufficient money");
            }
        }
        else
        {
            Debug.Log("Product does not exist: " + productName);
        }
    }

    // Method to add a product to the dictionary
    public void AddProduct(ShopProduct product)
    {
        productDict.Add(product.ProductName, product);
    }
}
