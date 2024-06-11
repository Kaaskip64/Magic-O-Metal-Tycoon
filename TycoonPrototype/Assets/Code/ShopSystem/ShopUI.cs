using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    // UI Elements
    public Text MoneyTextShopPanel;
    public Text MoneyTextStageUI;
    public Transform category;
    public Transform productParent;
    public GameObject ShopDisplayArea;
    public Button closeButton;
    public GameObject hoverPanel;
    public float hoverOffsetX;
    public float hoverOffsetY;

    // Lists and Dictionary
    private List<Button> categoryButtons = new List<Button>();
    private List<GameObject> productAreas = new List<GameObject>();
    private Dictionary<Button, GameObject> ProductPair = new Dictionary<Button, GameObject>();

    // Miscellaneous
    private Button selectedCategoryButton;

    private bool isHoverOn;


    void Start()
    {
        PlayerProperties.MoneyAction += MoneyTextChange;

        UIBehaviourInit();

        MoneyTextChange(PlayerProperties.Instance.money);
    }

    private void UIBehaviourInit()
    {
        for (int i = 0; i < category.childCount; i++)
        {
            categoryButtons.Add(category.GetChild(i).GetComponent<Button>());
        }

        for (int i = 0; i < productParent.childCount; i++)
        {
            productAreas.Add(productParent.GetChild(i).gameObject);
        }

        //To make sure each button is corresponding to a non-null category
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

        closeButton.onClick.AddListener(CloseMenu);
    }

    private void Update()
    {
        if(isHoverOn)
        {
            hoverPanel.transform.position = Input.mousePosition + new Vector3(hoverOffsetX,hoverOffsetY,0);
        }
    }

    private void MoneyTextChange(float currentMoney)
    {
        MoneyTextShopPanel.text = currentMoney.ToString();
        MoneyTextStageUI.text = currentMoney.ToString();
    }

    private void OnCategoryButtonClick(Button categoryButton)
    {
        foreach (GameObject category in productAreas)
        {
            category.SetActive(false);
        }

        ProductPair[categoryButton].SetActive(true);

        HighlightCategoryButton(categoryButton);

        ShopDisplayArea.SetActive(true);

        GetComponent<ShopSystem>().PlayClickSound();
    }

    private void HighlightCategoryButton(Button selectedButton)
    {
        if (selectedCategoryButton != null)
        {
            selectedCategoryButton.image.color = Color.black;
        }

        selectedButton.image.color = Color.red;

        selectedCategoryButton = selectedButton;
    }

    private void CloseMenu()
    {
        ShopDisplayArea.SetActive(false);
        selectedCategoryButton.image.color = Color.black;
    }

    public void DisableCategoryButton() // function to disable the category button
    {
        foreach (var button in categoryButtons)
        {
            button.interactable = false;
        }
    }

    public void EnableCategoryButton() // function to enable the category button
    {
        foreach (var button in categoryButtons)
        {
            button.interactable = true;
        }
    }

    public void ShowHoverPanel(ShopProduct product)
    {
        isHoverOn = true;
        hoverPanel.SetActive(true);

        hoverPanel.transform.Find("Name").GetComponent<Text>().text = product.ProductName;
        hoverPanel.transform.Find("Price").GetComponent<Text>().text = product.Price.ToString();


    }

    public void HideHoverPanel()
    {
        isHoverOn = false;
        hoverPanel.SetActive(false);
    }
}
