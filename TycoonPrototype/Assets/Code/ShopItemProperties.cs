using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItemProperties : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Canvas parentCanvas;

    public GameObject panel;

    public ShopPropertiesPanelValues values;
    
    public bool isOnUI;
    [Range(0, 1)]    
    public float xOffset;
    [Range(0, 1)]    
    public float YOffset;

    public RectTransform Rtransform;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        Vector2 pos;
        Rtransform = panel.GetComponent<RectTransform>();
        values = panel.GetComponent<ShopPropertiesPanelValues>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform, Input.mousePosition,
            parentCanvas.worldCamera,
            out pos);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isOnUI)
        {
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentCanvas.transform as RectTransform,
                Input.mousePosition, parentCanvas.worldCamera,
                out movePos);
            
            panel.transform.position = parentCanvas.transform.TransformPoint(movePos);
            Rtransform.pivot = new Vector2(xOffset, YOffset);
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        values.updatePanel();
        panel.SetActive(true);
        isOnUI = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
  
         isOnUI = false;
         panel.SetActive(false);
     }

   
}