using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float panSpeed = 20f;
    public float panBorderThickness = 10f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.mousePosition.y >= Screen.height - panBorderThickness)
        {
            pos.x -= panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.y <= panBorderThickness)
        {
            pos.x += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x >= Screen.width - panBorderThickness)
        {
            pos.z += panSpeed * Time.deltaTime;
        }
        if (Input.mousePosition.x <= panBorderThickness)
        {
            pos.z -= panSpeed * Time.deltaTime;
        }

        transform.position = pos;
    }
}
