using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandMember : MonoBehaviour
{
    // Start is called before the first frame update
    public Stage stage;

    Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();   
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if(stage.isPlaying)
        {
            animator.SetBool("isPlaying", true);
        }
        else
        {
            animator.SetBool("isPlaying", false);
        }
    }
}
