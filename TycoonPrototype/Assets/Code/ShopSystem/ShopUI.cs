using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text MoneyText; // Reference to the UI text for displaying money

    public Transform category; // Reference to the parent transform of category buttons
    private List<Button> categoryButtons = new List<Button>(); // List to store category buttons
    public Transform productParent; // Reference to the parent transform of product areas
    private List<GameObject> productAreas = new List<GameObject>(); // List to store product areas

    private Dictionary<Button, GameObject> ProductPair = new Dictionary<Button, GameObject>(); // Dictionary to store mapping between category buttons and product areas

    public GameObject PurchaseConfirmPanel; // Reference to the purchase confirmation panel
    public GameObject ShopDisplayArea; // Reference to the main shop display area

    public Button closeButton; // Reference to the close button in the UI

    private Button selectedCategoryButton; // Track the currently selected category button

    private ShopProduct currentProduct; // Reference to the currently selected product

    void Start()
    {
        // Subscribe to the MoneyAction event to update money text
        PlayerPropertiesTest.MoneyAction += MoneyTextChange;

        // Populate categoryButtons list with references to category buttons
        for (int i = 0; i < category.childCount; i++)
        {
            categoryButtons.Add(category.GetChild(i).GetComponent<Button>());
        }

        // Populate productAreas list with references to product areas
        for (int i = 0; i < productParent.childCount; i++)
        {
            productAreas.Add(productParent.GetChild(i).gameObject);
        }

        // Associate each category button with its corresponding product area
        foreach (Button button in categoryButtons)
        {
            button.onClick.AddListener(() => OnCategoryButtonClick(button)); // Add listener to category button click event
            string categoryName = button.GetComponentInChildren<Text>().text; // Get category name
            foreach (GameObject category in productAreas)
            {
                if (category.name == categoryName)
                {
                    ProductPair.Add(button, category); // Add mapping to dictionary
                }
            }
        }

        // Add listeners to purchase confirmation panel buttons
        PurchaseConfirmPanel.transform.Find("Cancel").GetComponent<Button>().onClick.AddListener(ClosePurchaseConfirmPanel);

        // Add listener to close button
        closeButton.onClick.AddListener(CloseMenu);
    }

    // Method to update money text
    private void MoneyTextChange(int currentMoney)
    {
        MoneyText.text = currentMoney.ToString();
    }

    // Method to handle category button clicks
    private void OnCategoryButtonClick(Button categoryButton)
    {
        // Deactivate all product areas
        foreach (GameObject category in productAreas)
        {
            category.SetActive(false);
        }

        // Activate corresponding product area
        ProductPair[categoryButton].SetActive(true);

        // Highlight the selected category button
        HighlightCategoryButton(categoryButton);

        ShopDisplayArea.SetActive(true);
    }

    // Method to highlight the selected category button
    private void HighlightCategoryButton(Button selectedButton)
    {
        if (selectedCategoryButton != null)
        {
            selectedCategoryButton.image.color = Color.black; 
        }

        // Change the background color of the newly selected category button
        selectedButton.image.color = Color.red;

        // Update the selected category button
        selectedCategoryButton = selectedButton;
    }

    // Method to close the menu
    private void CloseMenu()
    {
        // Close the menu
        ShopDisplayArea.SetActive(false);
        selectedCategoryButton.image.color = Color.black; // Reset color of selected category button
    }

    // Method to display purchase confirmation panel with information of the selected product
    public void ShowPurchaseConfirmPanel(ShopProduct currentProduct)
    {
        this.currentProduct = currentProduct;

        // Update information in the purchase confirmation panel
        var informationParent = PurchaseConfirmPanel.transform.Find("ProductInformation");
        informationParent.GetComponent<Image>().sprite = currentProduct.GetComponent<Image>().sprite;
        informationParent.Find("Name").GetComponent<Text>().text = "Name: " + currentProduct.ProductName;
        informationParent.Find("Price").GetComponent<Text>().text = "Price: " + currentProduct.Price.ToString();
        informationParent.Find("Stock").GetComponent<Text>().text = "Stock: " + currentProduct.Stock.ToString();

        PurchaseConfirmPanel.SetActive(true);
    }

    // Method to close the purchase confirmation panel
    public void ClosePurchaseConfirmPanel()
    {
        PurchaseConfirmPanel.SetActive(false);
    }

}
