using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopSystem : MonoBehaviour
{
    public static UnityAction<int> SuccessfulPurchase; // Event to notify successful purchase
    public static Func<ShopProduct, bool> MoneyCheck; // Function to check if player has enough money

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

            if (product.Stock > 0) // Check if there is enough stock
            {
                if (MoneyCheck != null && MoneyCheck(product)) // Check if player has enough money
                {
                    BuildingSystem.currentInstance.InitializeWithBuilding(product.itemPrefab); // Initialize with building
                    product.ChangeStock(-1); // Reduce stock
                    SuccessfulPurchase(-(int)product.Price); // Notify successful purchase
                    Debug.Log("Purchase product: " + productName);
                    Debug.Log("Product stock: " + product.Stock);
                }
                else
                {
                    Debug.Log("Insufficient money");
                }
            }
            else
            {
                Debug.Log("Insufficient stock: " + productName);
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

    // Method to start the purchase process
    public void StartPurchase(string currentProductName)
    {
        shopUI.ShowPurchaseConfirmPanel(productDict[currentProductName]);
    }

    // Method to confirm the purchase
    public void ConfirmPurchase()
    {
        // Implement logic to confirm the purchase
    }

    // Method to adjust the amount of a product
    public void AdjustProductAmount()
    {
        // Implement logic to adjust product amount
    }
}
