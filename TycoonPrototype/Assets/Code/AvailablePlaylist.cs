using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailablePlaylist : MonoBehaviour
{
    public List<Song> availableSongs;

    private void Start()
    {
        foreach (Song song in availableSongs)
        {
            song.songLength = song.songFile.length;
        }
    }
}
