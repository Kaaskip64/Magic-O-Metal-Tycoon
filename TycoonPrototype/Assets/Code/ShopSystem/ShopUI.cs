using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text MoneyText;

    public Transform category;
    private List<Button> categoryButtons = new();
    public Transform productParent;
    private List<GameObject> productAreas = new();

    private Dictionary<Button, GameObject> ProductPair = new();

    public GameObject PurchaseConfirmPanel;
    void Start()
    {
        PlayerPropertiesTest.MoneyAction += MoneyTextChange;

        for(int i = 0;i<category.childCount;i++)
        {
            categoryButtons.Add(category.GetChild(i).GetComponent<Button>());
        }

        for(int i = 0; i<productParent.childCount;i++)
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
    }


    public void ClosePurchaseConfirm()
    {
        PurchaseConfirmPanel.SetActive(false);
    }

}
