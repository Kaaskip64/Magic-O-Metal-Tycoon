using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;

public class BandDataTransferScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> nodes;
    
    public void UpdateListings(GameObject NewListItem)
    {
        nodes.Add(NewListItem);
    }

    public List<GameObject> GetNodesList()
    {
        return nodes;
    }

    public void ResetListings()
    {
        foreach (GameObject node in nodes)
        {
            Destroy(node);
        }
    }
}
