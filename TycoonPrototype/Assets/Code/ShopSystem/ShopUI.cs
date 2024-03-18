using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text MoneyText;

    public Transform category;
    private List<Button> categoryButtons = new List<Button>();
    public Transform productParent;
    private List<GameObject> productAreas = new List<GameObject>();

    private Dictionary<Button, GameObject> ProductPair = new Dictionary<Button, GameObject>();

    public GameObject PurchaseConfirmPanel;
    public GameObject ShopDisplayArea;

    public Button closeButton; // Reference to the close button in the UI

    private Button selectedCategoryButton; // Track the currently selected category button

    private ShopProduct currentProduct;

    void Start()
    {
        PlayerPropertiesTest.MoneyAction += MoneyTextChange;

        for (int i = 0; i < category.childCount; i++)
        {
            categoryButtons.Add(category.GetChild(i).GetComponent<Button>());
        }

        for (int i = 0; i < productParent.childCount; i++)
        {
            productAreas.Add(productParent.GetChild(i).gameObject);
        }

        foreach (Button button in categoryButtons)
        {
            button.onClick.AddListener(() => OnCategoryButtonClick(button));
            string categoryName = button.GetComponentInChildren<Text>().text;
            foreach (GameObject category in productAreas)
            {
                if (category.name == categoryName)
                {
                    ProductPair.Add(button, category);
                }
            }
        }

        PurchaseConfirmPanel.transform.Find("Confirm").GetComponent<Button>().onClick.AddListener(PressConfirm);
        PurchaseConfirmPanel.transform.Find("Cancel").GetComponent<Button>().onClick.AddListener(ClosePurchaseConfirmPanel);
    }

    private void MoneyTextChange(int currentMoney)
    {
        MoneyText.text = currentMoney.ToString();
    }

    // Method to handle category button clicks
    private void OnCategoryButtonClick(Button categoryButton)
    {
        foreach (GameObject category in productAreas)
        {
            category.SetActive(false);
        }

        ProductPair[categoryButton].SetActive(true);

        // Highlight the selected category button
        HighlightCategoryButton(categoryButton);

        // Activate the menu
        ShopDisplayArea.SetActive(true);
    }

    // Method to highlight the selected category button
    private void HighlightCategoryButton(Button selectedButton)
    {
        // Reset the appearance of the previously selected category button
        if (selectedCategoryButton != null)
        {
            // Reset the color of the previously selected button
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
        // Shrink the menu back to its original size
        ShopDisplayArea.SetActive(false); // Hide the menu after shrinking
    }
    public void ShowPurchaseConfirmPanel(ShopProduct currentProduct)
    {
        this.currentProduct = currentProduct;

        var informationParent = PurchaseConfirmPanel.transform.Find("ProductInformation");

        informationParent.GetComponent<Image>().sprite = currentProduct.GetComponent<Image>().sprite;
        informationParent.Find("Name").GetComponent<Text>().text = "Name: " + currentProduct.ProductName;
        informationParent.Find("Price").GetComponent<Text>().text = "Price: " + currentProduct.Price.ToString();
        informationParent.Find("Stock").GetComponent<Text>().text = "Stock: " + currentProduct.Stock.ToString();

        PurchaseConfirmPanel.SetActive(true);

        
    }



    public void ClosePurchaseConfirmPanel()
    {
        PurchaseConfirmPanel.SetActive(false);
    }

    private void PressConfirm()
    {
        currentProduct.PurchaseConfirm();
        ClosePurchaseConfirmPanel();
    }

}
