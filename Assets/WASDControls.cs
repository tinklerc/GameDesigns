using System.Collections;
using UnityEngine;

public class WASDControls : MonoBehaviour
{
    private float maxSpeed = 100;

    private int zoomSpeed = 5;

    private float maxZoomOut = 60;

    private float maxZoomIn = 5;

    void Update()
    {
        var time = Time.deltaTime;
        var vector = GetBaseInput();
        var distance = vector * time;
        transform.position += distance;
    }

    private Vector3 GetBaseInput()
    {
        //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3(0, 0, 0);
        var currentZoom = Camera.main.orthographicSize;
        if (Input.GetKey(KeyCode.W))
        {
            p_Velocity += new Vector3(0.0f, maxSpeed, 0.0f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            p_Velocity += new Vector3(0.0f, -maxSpeed, 0.0f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            p_Velocity += new Vector3(-maxSpeed, 0.0f, 0.0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            p_Velocity += new Vector3(maxSpeed, 0.0f, 0.0f);
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && currentZoom < maxZoomOut)
        {
            Camera.main.orthographicSize += zoomSpeed;
        }
        else if (
            Input.GetAxis("Mouse ScrollWheel") > 0f && currentZoom > maxZoomIn
        )
        {
            Camera.main.orthographicSize -= zoomSpeed;
        }
        return p_Velocity;
    }
}
