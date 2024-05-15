using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class mouseOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Canvas parentCanvas;

    public GameObject panel;

    public bool isOnUI;
    public int xOffset;

    public int YOffset;
    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
        Vector2 pos;

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
            
        }
        
    }

   public void OnPointerEnter(PointerEventData eventData)
    {
    
        panel.SetActive(true);
        isOnUI = true;
    }

   public void OnPointerExit(PointerEventData eventData)
   {
  
       isOnUI = false;
       panel.SetActive(false);
   }

   
}
