using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ShopSystem : MonoBehaviour
{
    public static UnityAction<int> SuccessfulPurchase;
    public static Func<ShopProduct,bool> MoneyCheck;
    private Dictionary<string, ShopProduct> productDict = new Dictionary<string, ShopProduct>();

    public void PurchaseProduct(string productName)
    {
        if (productDict.ContainsKey(productName))
        {   
            ShopProduct product = productDict[productName];
            //if(Money - product.Price>=0) //if I can directly access the money from other script
            //if (product.Category != ShopProduct.ProductCategory.Default)

            if (product.Stock > 0)
            {
                if(MoneyCheck!=null&&MoneyCheck(product))
                {
                    BuildingSystem.current.InitializeWithBuilding(product.itemPrefab);
                    product.ChangeStock(-1);
                    SuccessfulPurchase(-(int)product.Price);
                    Debug.Log("Purchase product��" + productName);
                    Debug.Log("Product stock: " + product.Stock);
                }
                else
                {
                    print("Insufficient money");
                }

            }
            else
            {
                Debug.Log("Insufficient stock" + productName);
            }


        }
        else
        {
            Debug.Log("Product not exist��" + productName);
        }
    }

    public void AddProduct(ShopProduct product)
    {
        productDict.Add(product.Name, product);
    }
}
