using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PriceHoverPanel : MonoBehaviour
{
    public GameObject hoverPanel;
    public Text priceAmount;

    public float hoverOffsetX;
    public float hoverOffsetY;

    private StageBuilder stageBuilder;

    private void Start()
    {
        stageBuilder = StageBuilder.currentInstance;
    }

    private void Update()
    {
        if(stageBuilder.isDragging)
        {
            ShowPricePanel();
        } else
        {
            HidePricePanel();
        }
    }

    public void ShowPricePanel()
    {
        hoverPanel.SetActive(true);
        hoverPanel.transform.position = Input.mousePosition + new Vector3(hoverOffsetX, hoverOffsetY, 0);
        priceAmount.text = stageBuilder.currentDragSelectionPrice.ToString();
    }

    public void HidePricePanel()
    {
        hoverPanel.SetActive(false);
    }
}
