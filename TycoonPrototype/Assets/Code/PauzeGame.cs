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
    public GameObject quitButton;
    
    
    
        
    public void Pauze()
    {
        isPauzed = !isPauzed;
        if (isPauzed)
        {
            quitButton.SetActive(true);
            image.sprite = imageList[0];
            Time.timeScale = 0;
        }
        else
        {
            quitButton.SetActive(false);
            image.sprite = imageList[1];
            Time.timeScale = 1;
            
        }
    }
}
