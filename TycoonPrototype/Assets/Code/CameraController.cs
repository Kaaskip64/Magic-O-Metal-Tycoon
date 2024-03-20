using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    public int minZoom = 5;
    public int maxZoom = 30;

    public float zoomSpeed = 10;
    public float zoomSensitivity = 5f;

    private float zoomTarget;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        zoomTarget = Camera.main.orthographicSize;
    }

    void Update()
    {
        Vector3 pos = transform.position;
        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        transform.position = pos;

        zoomTarget -= Input.mouseScrollDelta.y * zoomSensitivity;
        zoomTarget = Mathf.Clamp(zoomTarget, minZoom, maxZoom);
        float newSize = Mathf.MoveTowards(Camera.main.orthographicSize, zoomTarget, zoomSpeed * Time.deltaTime);
        Camera.main.orthographicSize = newSize;

    }

    
}
