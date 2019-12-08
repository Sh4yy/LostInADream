using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float pitch = 2;
    private float currentZoom = 10;
    public float zoomSpeed = 4f;
    public float minZoom = 5;
    public float maxZoom = 15;

    public float yawSpeed = 100f;
    public float pitchSpeed = 10f;
    private float currentYaw = 0f;
    private float currentPitch = 0f;

    private void Update()
    {
        currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);

    }

    private void LateUpdate()
    {
        transform.position = target.position - offset * currentZoom;
        transform.LookAt(target.position + Vector3.up * pitch);
        transform.RotateAround(target.position, Vector3.up, currentYaw);
        // transform.RotateAround(target.position, Vector3.left, currentPitch);
        transform.position += transform.up * currentPitch * 0.2f;
    }
}
