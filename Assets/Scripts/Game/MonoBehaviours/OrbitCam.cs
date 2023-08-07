using UnityEngine;

public class OrbitCam : MonoBehaviour
{
    private Transform trackedObject;
    public Transform TrackedObject;

    private Vector3 _trackedPosition;
    private Vector3 trackedPosition => TrackedObject?.position ?? Vector3.zero;

    public float xAngle = -23;
    public float yAngle = 45;
    public float zoomAmount = 40;
    public float lookSensitivity = 10;
    public float zoomSensitivity = 20;
    public float minZoom = 50;
    public float maxZoom = 50;

    void Update()
    {
        if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftControl))
        {
            xAngle += Input.GetAxis("Mouse X") * lookSensitivity * Time.deltaTime;
            yAngle -= Input.GetAxis("Mouse Y") * lookSensitivity * .5f * Time.deltaTime;
            yAngle = Mathf.Clamp(yAngle, -1, 1);
        }

        if (TrackedObject != null)
        {
            UpdateOrbit();
        }

        UpdateZoom();
    }

    void UpdateOrbit()
    {
        var xPosH = Mathf.Sin(xAngle) * zoomAmount;
        var zPosH = Mathf.Cos(xAngle) * zoomAmount;
        var yPosV = Mathf.Sin(yAngle) * zoomAmount;
        var posH = new Vector3(xPosH, yPosV, zPosH);
        transform.position = TrackedObject.position + posH;
        transform.LookAt(trackedPosition);
    }

    void UpdateZoom()
    {
        var scroll = Input.mouseScrollDelta.y;
        var isZoomIn = scroll > 0;
        var isZoomOut = scroll < 0;
        var zoomFactor = 100 + (zoomAmount * .003f) * zoomSensitivity;

        if (isZoomIn && zoomAmount > minZoom && zoomAmount > 5)
            zoomAmount -= zoomFactor * Time.deltaTime;
        if (isZoomOut && zoomAmount < maxZoom && zoomAmount < 100)
            zoomAmount += zoomFactor * Time.deltaTime;

        zoomAmount = Mathf.Clamp(zoomAmount, minZoom, maxZoom);
    }
}