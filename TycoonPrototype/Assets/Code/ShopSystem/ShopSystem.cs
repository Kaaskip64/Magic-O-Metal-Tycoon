using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopSystem : MonoBehaviour
{
    public GameObject upperBackground;

    private Dictionary<string, ShopProduct> productDict = new Dictionary<string, ShopProduct>(); // Dictionary to store products

    private ShopUI shopUI; // Reference to the ShopUI script

    private void Start()
    {
        shopUI = GetComponent<ShopUI>(); // Get reference to ShopUI component
        BuildingSystem.currentInstance.ExitBuildingFollowing += shopUI.EnableCategoryButton; // forwarding Enable button function from shopUI to BuildingSystem
    }

    // Method to handle purchasing a product
    public void SelectProduct(string productName)
    {
        if (productDict.ContainsKey(productName)) // Check if the product exists
        {
            ShopProduct product = productDict[productName]; // Get the product
            BuildingSystem.currentInstance.InitializeWithBuilding(product); // Initialize with building
            upperBackground.SetActive(false);
            shopUI.DisableCategoryButton();
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
