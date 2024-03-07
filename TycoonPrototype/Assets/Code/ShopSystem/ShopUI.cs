using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    public Text MoneyText;
    public List<Button> CategoryButtons; // Add references to your category buttons in the Inspector
    public List<GameObject> ProductAreas;

    private Dictionary<Button, GameObject> ProductPair = new();

    void Start()
    {
        PlayerPropertiesTest.MoneyAction += MoneyTextChange;

        foreach (Button button in CategoryButtons)
        {
            button.onClick.AddListener(() => OnCategoryButtonClick(button));
            string categoryName = button.GetComponentInChildren<Text>().text;
            foreach (GameObject category in ProductAreas)
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
        MoneyText.text = "Money: " + currentMoney;
    }



    // Method to handle category button clicks
    private void OnCategoryButtonClick(Button categoryButton)
    {
        foreach (GameObject category in ProductAreas)
        {
            category.SetActive(false);
        }

        ProductPair[categoryButton].SetActive(true);
    }




}
