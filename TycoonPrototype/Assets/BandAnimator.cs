using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandAnimator : MonoBehaviour
{

    public Animator animator;
    
    // Start is called before the first frame update


    public void changeStatus(bool isPlaying)
    {
        animator.SetBool("isPlaying", isPlaying);
    }
    
}
