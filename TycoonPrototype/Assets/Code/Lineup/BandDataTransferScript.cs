using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class BandDataTransferScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> nodes;

    [SerializeField] private GameObject freshListItem;

    public delegate void NewBandAdded();

    public NewBandAdded newbandAdded;
    public void UpdateListings(GameObject NewListItem)
    {
        nodes.Add(NewListItem);
        
        if(newbandAdded != null){newbandAdded();}
    }
    public List<BandListingData> GetNodesList()
    {
        List<BandListingData> datalist = new List<BandListingData>();
        foreach (GameObject node in nodes)
        {
            datalist.Add(node.GetComponent<NewBandData>().GetNewBandData());
        }
        return datalist;
    }

    public void StartNewLineUp()
    {

        GameObject NewBand = Instantiate(freshListItem, this.transform);
        nodes.Add(NewBand);
        UpdateListings(NewBand);
    }

    public void ResetListings()
    {
        foreach (GameObject node in nodes)
        {
            Destroy(node);
        }
        nodes.Clear();
    }

    public async void UploadLineUp(List<BandListingData> items)
    {
        foreach (BandListingData data in items)
        {
           await AddNewBandAddressable(data);
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            if (i != nodes.Count)
            {
                nodes[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
    
    public async Task AddNewBandAddressable(BandListingData data)
    {
        AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>("BandPrefab");
        await goHandle.Task;
        if (goHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = goHandle.Result;
            if (this != null && transform != null && transform != null)
            {
                GameObject BandOpening =  Instantiate(obj, transform);
                BandOpening.GetComponent<NewBandData>().setNewBandData(data);
                UpdateListings(BandOpening);
            }
            else
            {
                Debug.LogError("Invalid transform hierarchy or script has been destroyed.");
            }
        }
        else
        {
            Debug.LogError($"Loading failed with status: {goHandle.Status}");
            if (goHandle.OperationException != null)
            {
                Debug.LogError($"Exception: {goHandle.OperationException}");
            }
        }
    }
}
