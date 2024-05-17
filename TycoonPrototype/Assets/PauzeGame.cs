using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
public class PauzeGame : MonoBehaviour
{
    public bool isPauzed = false;
    public List<Sprite> imageList;
    public Image image;


    public void Pauze()
    {
        isPauzed = !isPauzed;
        if (isPauzed)
        {
            image.sprite = imageList[0];
            Time.timeScale = 0;
            
        }
        else
        {
            image.sprite = imageList[1];
            Time.timeScale = 1;
            
        }
    }
}
