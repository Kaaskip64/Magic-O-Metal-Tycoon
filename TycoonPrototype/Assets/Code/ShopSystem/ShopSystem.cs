using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour
{
    public GameObject upperBackground;
    public AudioSource audioSource;

    [Header("Sound Effects")]
    public AudioClip clickSound;    
    public AudioClip hoverSound;
    public AudioClip scrollSound;

    private Dictionary<string, ShopProduct> productDict = new Dictionary<string, ShopProduct>(); // Dictionary to store products
    private ShopUI shopUI; // Reference to the ShopUI script

    private void Start()
    {
        shopUI = GetComponent<ShopUI>(); // Get reference to ShopUI component
        BuildingSystem.currentInstance.ExitBuildingFollowing += shopUI.EnableCategoryButton; // forwarding Enable button function from shopUI to BuildingSystem

        // add scroll bar path to detect scroll value change
        AddScrollbarListeners("UpperBackground/ProductArea/Stage/Scrollbar Vertical");
        AddScrollbarListeners("UpperBackground/ProductArea/Facilities/Scrollbar Vertical");
        AddScrollbarListeners("UpperBackground/ProductArea/Decoration/Scrollbar Vertical");
        AddScrollbarListeners("UpperBackground/ProductArea/Lineup/Scrollbar Vertical");
        AddScrollbarListeners("UpperBackground/ProductArea/Player Import/Scrollbar Vertical");
    }

    // Method to handle purchasing a product
    public void SelectProduct(string productName)
    {
        if (productDict.ContainsKey(productName)) // Check if the product exists
        {
            HideHoverPanel();
            ShopProduct product = productDict[productName]; // Get the product
            BuildingSystem.currentInstance.InitializeWithBuilding(product); // Initialize with building
            upperBackground.SetActive(false);
            shopUI.DisableCategoryButton();
            PlayClickSound();
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

    public void ShowHoverPanel(ShopProduct product)
    {
        shopUI.ShowHoverPanel(product);
        PlayHoverSound();
    }

    public void HideHoverPanel()
    {
        shopUI.HideHoverPanel();
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }

    private void PlayHoverSound()
    {
        if (audioSource != null && hoverSound != null)
        {
            audioSource.PlayOneShot(hoverSound);
        }
    }

    private void AddScrollbarListeners(string path)
    {
        Transform scrollbarTransform = transform.Find(path);
        if (scrollbarTransform != null)
        {
            Scrollbar scrollbar = scrollbarTransform.GetComponent<Scrollbar>();
            if (scrollbar != null)
            {
                scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
            }
            else
            {
                Debug.LogWarning("No Scrollbar component found at path: " + path);
            }
        }
        else
        {
            Debug.LogWarning("No Transform found at path: " + path);
        }
    }

    private void OnScrollbarValueChanged(float value)
    {
        if (audioSource != null && scrollSound != null)
        {
            audioSource.PlayOneShot(scrollSound);
        }
        // change logic here to tweak behaviour of scroll sound
    }
}
