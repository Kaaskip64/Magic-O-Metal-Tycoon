using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBandData : MonoBehaviour
{
    [SerializeField]
    private BandListingData data;

    public void setNewBandData(BandListingData newData)
    {
        data = newData;
    }

    public BandListingData GetNewBandData()
    {
        return data;
    }
}
