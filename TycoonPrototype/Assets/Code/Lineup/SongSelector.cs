using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SongSelector : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject currentNodeSelected;
    [SerializeField] private BandListings bandListings;
    [SerializeField] private List<GameObject> musicDataList;
    
    public void OnPointerClick(PointerEventData pointerEventData)
    {
        Debug.Log(pointerEventData);
    }
    
    
    public GameObject getCurrentNodeSelected()
    {
        return currentNodeSelected;
    }
    
    public void SetCurrentNodeSelected(GameObject newNode)
    {
        currentNodeSelected = newNode;
    }
}
