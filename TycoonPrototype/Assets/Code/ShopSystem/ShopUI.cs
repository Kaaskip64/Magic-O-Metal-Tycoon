using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text MoneyText;
    public Button[] CategoryButtons; // Add references to your category buttons in the Inspector
    public Transform ProductsContainer; // The container where product buttons will be displayed
    public GameObject ProductButtonPrefab; // Prefab for product buttons

    private ShopProduct.ProductCategory currentCategory = ShopProduct.ProductCategory.Default;

    void Start()
    {
        PlayerPropertiesTest.MoneyAction += MoneyTextChange;

        // Subscribe to button click events for each category button
        foreach (Button categoryButton in CategoryButtons)
        {
            categoryButton.onClick.AddListener(() => OnCategoryButtonClick(categoryButton));
        }

        // Populate products initially
        UpdateProductDisplay();
    }

    private void MoneyTextChange(int currentMoney)
    {
        MoneyText.text = "Money: " + currentMoney;
    }

    // Method to update the displayed products based on the selected category
    private void UpdateProductDisplay()
    {
        // Clear existing product buttons
        foreach (Transform child in ProductsContainer)
        {
            Destroy(child.gameObject);
        }

        // Get products based on the selected category
        ShopProduct[] products = GetProductsByCategory(currentCategory);

        // Instantiate product buttons and set them up
        foreach (ShopProduct product in products)
        {
            GameObject buttonObject = Instantiate(ProductButtonPrefab, ProductsContainer);
            ProductButton productButton = buttonObject.GetComponent<ProductButton>();
            productButton.Initialize(product);
        }
    }

    // Method to handle category button clicks
    private void OnCategoryButtonClick(Button categoryButton)
    {
        // Get the category from the button's text (assuming the text contains the category name)
        string categoryName = categoryButton.GetComponentInChildren<Text>().text;

        // Convert the string to the corresponding enum value
        if (System.Enum.TryParse(categoryName, out ShopProduct.ProductCategory category))
        {
            currentCategory = category;
            UpdateProductDisplay();
        }
        else
        {
            Debug.LogError("Invalid category: " + categoryName);
        }
    }

    // Method to filter products based on the selected category
    private ShopProduct[] GetProductsByCategory(ShopProduct.ProductCategory category)
    {
        // Replace this logic with your own method of filtering products based on the category
        return FindObjectsOfType<ShopProduct>().Where(product => product.Category == category).ToArray();
    }
}
