using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BandAdder : MonoBehaviour
{
    [SerializeField]
    private GameObject objectToSpawn;
    
    [SerializeField]
    private RectTransform ParentObject;
    
    [SerializeField]
    private GameObject objectToHide;
    public float price = 50f;
    
    public async Task AddNewBandAddressable()
    {
        AsyncOperationHandle<GameObject> goHandle = Addressables.LoadAssetAsync<GameObject>("BandPrefab");
        await goHandle.Task;
        if (goHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = goHandle.Result;
            if (this != null && transform.parent != null && transform.parent != null)
            {
               GameObject BandOpening =  Instantiate(obj, transform.parent);
               BandOpening.transform.parent.gameObject.GetComponent<BandDataTransferScript>().UpdateListings(BandOpening);
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
    public void AddNewBand()
    {
        if (PlayerProperties.Instance.MoneyCheck(price))
        {
            objectToHide.SetActive(false);
            AddNewBandAddressable();
            PlayerProperties.Instance.MoneyChange(-price);
        }

    }
    public void SetParent(RectTransform newParent)
  {
      ParentObject = newParent;
  }
    public RectTransform GetParent()
  {
      return ParentObject;
  }
  public void SetObjectToSpawn(GameObject newObject)
  {
      objectToSpawn = newObject;
  }
  
}
