using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductButton : MonoBehaviour
{
    public Text ProductNameText;
    public Text PriceText;
    public Button BuyButton;

    private ShopProduct product;

    public void Initialize(ShopProduct product)
    {
        //this.product = product;
        //ProductNameText.text = product.ProductName;
        //PriceText.text = "Price: " + product.Price;
        //BuyButton.onClick.AddListener(product.Purchase);
    }
}
