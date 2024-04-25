using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class BandListings : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> BandsAmount;

    public List<BandListingData> BandData;

    public delegate void BandlistingFilledEvent(List<GameObject> bandList);

    public event BandlistingFilledEvent bansListSorted;
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
    
    
    public async void SortListing()
    {
        foreach (BandListingData bandData in BandData)
        {   
            await AddNewBandAddressable();
        }

        if (bansListSorted != null)
        {
            bansListSorted(BandsAmount);
        }
        SortData();
    }

    public void SortData()
    {
        for(int i = 0; i <BandData.Count; i++)
        { 
            BandListingDataTranslator data = BandsAmount[i].GetComponent<BandListingDataTranslator>();
           data.data = BandData[i];
           data.translateData();
        }
    }

    public List<GameObject> getBandAmount()
    {
        return BandsAmount;
    }
    public void Start()
    {
        SortListing();
    }
}
