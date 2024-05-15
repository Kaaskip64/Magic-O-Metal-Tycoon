using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    public int minZoom = 5;
    public int maxZoom = 30;

    public float zoomSpeed = 10;
    public float zoomSensitivity = 5f;

    private float zoomTarget;

    public bool cameraActive;

    public GameObject ShopUiUpper;
    public GameObject ShopUiLower;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        zoomTarget = Camera.main.orthographicSize;
    }

    void Update()
    {
        if(cameraActive)
        {
            CameraMove();
            HandleCameraZoom();
        }

    }

    void CameraMove()
    {
        Vector3 pos = transform.position;
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
        transform.position = pos;


    }

    private void HandleCameraZoom()
    {
        // Check if the mouse scroll wheel is being used
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0)
        {
            // Check if the pointer is over a UI element
            if (!IsPointerOverUIElement())
            {
                // Zoom the camera
                zoomTarget -= Input.mouseScrollDelta.y * zoomSensitivity;
                zoomTarget = Mathf.Clamp(zoomTarget, minZoom, maxZoom);
                float newSize = Mathf.MoveTowards(Camera.main.orthographicSize, zoomTarget, zoomSpeed * Time.deltaTime);
                Camera.main.orthographicSize = newSize;
            }
        }
    }
/*
    private bool IsPointerOverUIElement()
    {
        // Check if the pointer is over any UI element
        return EventSystem.current.IsPointerOverGameObject();
    }*/

    private bool IsPointerOverUIElement()
    {
        // Check if the pointer is over any UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

            foreach (RaycastResult result in results)
            {
                if (result.gameObject == ShopUiUpper || result.gameObject == ShopUiLower)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
