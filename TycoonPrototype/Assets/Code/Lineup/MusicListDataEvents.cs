using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MusicListDataEvents : MonoBehaviour, IPointerClickHandler
{
    public BandListingDataTranslator translator;

    public delegate void TransferData(BandListingData data);

    public event TransferData dataTransfer;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        dataTransfer(translator.data);
        transform.parent.parent.parent.gameObject.SetActive(false);
    }
}
