using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MaintenanceTicks : MonoBehaviour
{
    public static MaintenanceTicks currentInstance;

    public float maintenanceTimer = 6f;

    public UnityEvent Tick;

    private void Awake()
    {
        currentInstance = this;
    }

    private void Start()
    {
        if (Tick == null)
        {
            Tick = new UnityEvent();
        }

        InvokeRepeating("CallBuildings", maintenanceTimer, maintenanceTimer);
    }

    private void CallBuildings()
    {
        Tick.Invoke();
    }
}
