using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewBandData : MonoBehaviour
{
    [SerializeField]
    private BandListingData data;

    [SerializeField] 
    private TextMeshProUGUI text;
        
    public void setNewBandData(BandListingData newData)
    {
        data = newData;
        text.text = data.BandName + " " + data.SongName;
    }

    public BandListingData GetNewBandData()
    {
        return data;
    }
    
}
