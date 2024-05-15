using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [Header("Panning")]
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    [Space(20)]

    [Header("Zoom Boundaries (dont make min bigger than max or suffer the consequences)")]
    [Range(5, 225)]
    public int minZoom = 5;
    [Range(5, 225)]
    public int maxZoom = 30;

    [Space(30)]
    [Header("Misc Zoom Variables")]
    public float zoomSpeed = 10;
    public float zoomSensitivity = 5f;

    [Header("Camera State")]
    public bool cameraActive;
    [Header("Virtual Camera")]
    public CinemachineVirtualCamera vcam;

    private float zoomTarget;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        zoomTarget = vcam.m_Lens.OrthographicSize;

        if(minZoom > maxZoom)
        {
            Debug.LogError("Camera zoom min is bigger than max. Now face my 1000 year mind prison attack");
        }
    }

    void Update()
    {
        if (minZoom > maxZoom)
        {
            return;
        }

        if(cameraActive)
        {
            CameraMove();
        }

    }

    void CameraMove()
    {
        Vector3 pos = Camera.main.transform.position;
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.y += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.y -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        vcam.transform.position = pos;

        zoomTarget -= Input.mouseScrollDelta.y * zoomSensitivity;
        zoomTarget = Mathf.Clamp(zoomTarget, minZoom, maxZoom);
        float newSize = Mathf.MoveTowards(vcam.m_Lens.OrthographicSize, zoomTarget, zoomSpeed * Time.deltaTime);
        vcam.m_Lens.OrthographicSize = newSize;
    }

    
}

