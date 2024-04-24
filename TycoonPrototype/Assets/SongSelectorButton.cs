using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SongSelectorButton : MonoBehaviour
{

    public Transform grandParent;

    public Button button;
    // Start is called before the first frame update
    void Start()
    {
        grandParent = transform.parent.parent;
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(setCurrentActiveButton);
    }

    public void setCurrentActiveButton()
    {
        SongSelector SSelector = grandParent.gameObject.GetComponent<SongSelector>();
        SSelector.SetCurrentNodeSelected(this.gameObject);
    }
    
}
