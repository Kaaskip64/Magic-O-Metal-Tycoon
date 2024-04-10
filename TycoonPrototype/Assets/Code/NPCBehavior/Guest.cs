using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guest : MonoBehaviour
{
    private AIDestinationSetter destinationSetter;

    private Transform destinationTransform;

    private void Awake()
    {
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    //set the target and move to it
    private void GoToTarget(Transform destination)
    {
        destinationTransform = destination;
    }





}
