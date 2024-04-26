using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BandListingDataTranslator : MonoBehaviour
{

    public BandListingData data;
    [SerializeField] private TextMeshProUGUI bandText;
    [SerializeField] private TextMeshProUGUI SongText;
    [SerializeField] private Image albumImage;

        // Start is called before the first frame update
   public void translateData()
    {
        bandText.text = data.BandName;
        SongText.text = data.SongName;
        albumImage.sprite = data.BandImage;
    }
}
