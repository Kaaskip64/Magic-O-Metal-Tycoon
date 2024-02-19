using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUnit : MonoBehaviour, ISelectable
{
    public GameManager manager;

    private void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    private void Update()
    {
        if(manager.currentSelection.Contains(gameObject))
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
        } else
        {
            gameObject.GetComponent<MeshRenderer>().material.color = Color.white;

        }
    }

    private void OnMouseDown()
    {
        Select();
    }


    public void Select()
    {
        if(!Input.GetKey(KeyCode.LeftControl))
        {
            manager.currentSelection.Clear();

        }
        manager.currentSelection.Add(gameObject);

    }
}
