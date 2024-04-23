using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class BandListings : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> BandsAmount;

    public List<ScriptableObject> BandData;

    public async Task AddNewBandAddressable()
    {
        AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>("BandNode");
        await goHandle.Task;
        
        if (goHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = goHandle.Result;
            if (this != null && transform != null && transform != null)
            {
              GameObject BandListing =   Instantiate(obj, this.transform);
              BandsAmount.Add(BandListing);
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
    
    
    public void SortListing()
    {
        foreach (ScriptableObject bandData in BandData)
        {
            AddNewBandAddressable();
            Debug.Log("hit");
        }  
    }

    public void Start()
    {
        SortListing();
    }
}
