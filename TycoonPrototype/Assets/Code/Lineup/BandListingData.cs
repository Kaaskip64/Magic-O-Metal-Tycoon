using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/BandData", order = 1)]
public class BandListingData : ScriptableObject
{
   public Sprite BandImage;
   public String BandName;
   public String SongName;
   public AudioClip MusicFile;
}
