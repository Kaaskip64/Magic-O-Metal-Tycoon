using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
public class BandDataTransferScript : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> nodes;

    [SerializeField] private GameObject freshListItem;
    public void UpdateListings(GameObject NewListItem)
    {
        nodes.Add(NewListItem);
    }
    public List<GameObject> GetNodesList()
    {
        return nodes;
    }

    public void StartNewLineUp()
    {

        GameObject NewBand = Instantiate(freshListItem,this.transform);
        nodes.Add(NewBand);
    }
    
    public void ResetListings()
    {
        foreach (GameObject node in nodes)
        {
            nodes.Remove(node);
            Destroy(node);
            
        }
    }

    public void UploadLineUp(List<BandListingData> items)
    {
        foreach (BandListingData data in items)
        {
            AddNewBandAddressable(data);
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
