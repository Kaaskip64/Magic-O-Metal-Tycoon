using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class SongSelector : MonoBehaviour
{
    [SerializeField]
    private GameObject currentNodeSelected;
    [SerializeField] private BandListings bandListings;
    public GameObject MusicSelection;
    private void Start()
    {
        bandListings.bansListSorted += AddListeners;
    }


    public void AddListeners(List<GameObject> bandList)
    {
        foreach (GameObject musicNode in bandList)
        {
          MusicListDataEvents mNode =  musicNode.GetComponent<MusicListDataEvents>();
          mNode.dataTransfer += SaveData;
        }
    }

    public void SaveData(BandListingData data)
    {
        currentNodeSelected.GetComponentInParent<NewBandData>().setNewBandData(data);
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
