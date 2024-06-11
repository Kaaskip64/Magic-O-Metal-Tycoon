using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class bandAmountDisplay : MonoBehaviour
{
    public GameObject bandList;
    public TextMeshProUGUI text;
    void Update()
    {
        if (bandList.transform.childCount == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            text.text = "Bands in Line-up: " + bandList.transform.childCount;
        }
    }
        
}
