using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ShopSystem : MonoBehaviour
{
    public GameObject upperBackground;
    public AudioSource audioSource;
    public List<Scrollbar> scrollbarObjs;
    public float deadZoneThreshold = 0.05f;

    [Header("Sound Effects")]

    [Tooltip("ButtonSounds")] 
    public AudioClip clickSound;    
    public AudioClip hoverSound;
    [Tooltip("ScrollbarSounds")]
    public AudioClip scrollEndSound;
    public AudioClip scrollChangeSound;
    public AudioClip scrollStopSound; // Sound to play when scrolling stops
    // public AudioClip scrollSound;

    public Dictionary<string, ShopProduct> productDict = new Dictionary<string, ShopProduct>(); // Dictionary to store products

    private Dictionary<Scrollbar, float> lastValues = new Dictionary<Scrollbar, float>();
    private Dictionary<Scrollbar, Coroutine> checkCoroutines = new Dictionary<Scrollbar, Coroutine>();

    private ShopUI shopUI; // Reference to the ShopUI script
    public static ShopSystem instance;

    private void Awake()
    {
        instance = this;
        
    }

    private void Start()
    {
        shopUI = GetComponent<ShopUI>(); // Get reference to ShopUI component
        BuildingSystem.currentInstance.ExitBuildingFollowing += shopUI.EnableCategoryButton; // forwarding Enable button function from shopUI to BuildingSystem

        foreach (var scrollbar in scrollbarObjs)
        {
            lastValues[scrollbar] = scrollbar.value;
            scrollbar.onValueChanged.AddListener((value) => OnScrollbarValueChanged(scrollbar, value));
        }
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

    public void PlayClickSound()
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

    /*private void AddScrollbarListeners(string path)
    {
        Transform scrollbarTransform = transform.Find(path);
        if (scrollbarTransform != null)
        {
            Scrollbar scrollbar = scrollbarTransform.GetComponent<Scrollbar>();

            if (scrollbar != null)
            {
                scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
                //Debug.Log("Added listener to scrollbar: " + path);
            }
            else
            {
                //Debug.LogWarning("No Scrollbar component found at path: " + path);
            }
        }
        else
        {
            //Debug.LogWarning("No Transform found at path: " + path);
        }
    }*/

    private void OnScrollbarValueChanged(Scrollbar scrollbar, float value)
    {
        float lastValue = lastValues[scrollbar];
        

        if (Mathf.Abs(value - lastValue) > deadZoneThreshold)
        {
            if (Mathf.Approximately(value, 1f) || Mathf.Approximately(value, 0f) ||
            Mathf.Abs(value - 1f) < deadZoneThreshold || Mathf.Abs(value) < deadZoneThreshold)
            {
                // Play end sound if the scrollbar is at the end or the start, with a deadzone
                audioSource.loop = false;
                audioSource.PlayOneShot(scrollEndSound);
            }
            else
            {
                // Play change sound if the scrollbar value has changed but not at the ends
                if (!audioSource.isPlaying || (audioSource.clip == scrollChangeSound && !audioSource.loop))
                {
                    audioSource.clip = scrollChangeSound;
                    audioSource.loop = true;
                    audioSource.Play();
                }

            }

            // Update the last value
            lastValues[scrollbar] = value;

            // Stop any existing coroutine for this scrollbar
            if (checkCoroutines.ContainsKey(scrollbar) && checkCoroutines[scrollbar] != null)
            {
                StopCoroutine(checkCoroutines[scrollbar]);
                
            }

            // Start a new coroutine to check if the value stops changing
            
            checkCoroutines[scrollbar] = StartCoroutine(CheckIfScrollbarStops(scrollbar));
        }
    }

    private IEnumerator CheckIfScrollbarStops(Scrollbar scrollbar)
    {
        float initialValue = scrollbar.value;

        // Wait for a short duration
        yield return new WaitForSeconds(0.1f);

        // Check if the value has not changed
        if (Mathf.Approximately(initialValue, scrollbar.value))
        {
            // Stop the looped change sound
            if (audioSource.loop && audioSource.isPlaying)
            {
                audioSource.loop = false;
                // Wait for the clip to finish playing
                while (audioSource.isPlaying)
                {
                    yield return null;
                }
            }
            // Play stop sound
            audioSource.PlayOneShot(scrollStopSound);
        }
        else
        {
            // If the value has changed, restart the coroutine to keep checking
            checkCoroutines[scrollbar] = StartCoroutine(CheckIfScrollbarStops(scrollbar));
        }
    }

    /*public void ChangeAudio(int eventNumber)
    {
        switch (eventNumber)
        {
            case 0:
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = scrollSounds[eventNumber];
                    audioSource.Play();
                }

                break;
            case 1:
                if (!audioSource.isPlaying)
                {
                    audioSource.clip = scrollSounds[eventNumber];
                    audioSource.Play();
                }

                break;
            case 2:
                audioSource.clip = scrollSounds[eventNumber];
                audioSource.Play();
                break;
        }
    
     }*/
}

