using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpawnManagerScriptableObject", order = 1)]
public class BandListingData : ScriptableObject
{
   public Image BandImage;
   public String BandName;
   public String AlbumName;
   public AudioClip MusicFile;
}
