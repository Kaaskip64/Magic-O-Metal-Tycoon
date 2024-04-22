using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Song", menuName = "ScriptableObjects/Song", order = 2)]

public class Song : ScriptableObject
{
    public string songName;
    public AudioClip songFile;

    public float songLength;

}

