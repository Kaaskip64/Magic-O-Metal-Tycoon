using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMusicEventHandler : MonoBehaviour
{
    public delegate void PlayStarted();

    public PlayStarted playStarted;

    [SerializeField] private BandDataTransferScript BandListingsList;
    [SerializeField] private List<GameObject> objectsToActivateToggle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddbuttonToList()
    {
        
    }
}
