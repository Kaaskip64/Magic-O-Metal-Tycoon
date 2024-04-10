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

    private void GoToTarget(Transform destination)
    {
        destinationTransform = destination;
    }





}
