using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanCam : MonoBehaviour
{
    private Vector3 speed = Vector3.zero;
    public Vector3 speedDeadZone = new Vector3(.1f, .1f, 0);
    public float acceleration = 0.1f;
    public float edgeSize = 25;
    public float maxSpeed = 5;
    public float braking = 1;
    public int zoomAmount = 25;
    public bool allowEdgeScroll = true;
    public bool invertWS = false;

    private bool isIdleX = false;
    private bool isIdleY = false;

    private bool getIsForwardScroll() => Input.mousePosition.y > (Screen.height - edgeSize) && allowEdgeScroll;
    private bool getIsPanForward()
    {
        if (!invertWS)
            return Input.GetKey(KeyCode.W) || getIsForwardScroll();
        else
            return Input.GetKey(KeyCode.S) || getIsBackwardScroll();
    }

    private bool getIsBackwardScroll() => Input.mousePosition.y < edgeSize && allowEdgeScroll;
    private bool getIsPanBackward()
    {
        if (!invertWS)
            return Input.GetKey(KeyCode.S) || getIsBackwardScroll();
        else
            return Input.GetKey(KeyCode.W) || getIsForwardScroll();
    }

    private bool getIsPanLeft()
        => Input.GetKey(KeyCode.A) || (Input.mousePosition.x < edgeSize && allowEdgeScroll);

    private bool getIsPanRight()
        => Input.GetKey(KeyCode.D) || (Input.mousePosition.x > (Screen.width - edgeSize) && allowEdgeScroll);

    float GetRealSpeed(float speed)
        => speed < 0 ? -speed : speed;

    void Update()
    {
        DoZoom();
        DoMovement();
        DoPassiveBraking();

        var tfX = !invertWS ? transform.forward.x : transform.forward.z;
        var tfZ = !invertWS ? transform.forward.z : transform.forward.x;

        var myForward = new Vector3(-tfZ, 0, -tfX);
        var myRight = new Vector3(-transform.right.x, 0, -transform.right.z);

        transform.Translate(myForward * speed.y * Time.deltaTime, Space.World);
        transform.Translate(myRight * speed.x * Time.deltaTime, Space.World);
    }

    void DoZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        bool isZoomIn = scroll > 0;
        bool isZoomOut = scroll < 0;

        if (isZoomIn)
            transform.Translate(Vector3.forward * zoomAmount * 4 * Time.deltaTime);

        if (isZoomOut)
            transform.Translate(Vector3.back * zoomAmount * 4 * Time.deltaTime);
    }

    void DoMovement()
    {
        bool isSprint = Input.GetKey(KeyCode.LeftShift);

        if (getIsPanForward() && speed.y < maxSpeed) speed.y += acceleration * (isSprint ? 2 : 1);
        if (getIsPanBackward() && speed.y > -maxSpeed) speed.y -= acceleration * (isSprint ? 2 : 1);
        if (getIsPanLeft() && speed.x < maxSpeed) speed.x += acceleration * (isSprint ? 2 : 1);
        if (getIsPanRight() && speed.x > -maxSpeed) speed.x -= acceleration * (isSprint ? 2 : 1);
    }

    void DoPassiveBraking()
    {
        isIdleX = !getIsPanLeft() && !getIsPanRight();
        isIdleY = !getIsPanForward() && !getIsPanBackward();

        if (isIdleX) speed.x = GetRealSpeed(speed.x) > speedDeadZone.x ? Mathf.Lerp(speed.x, speed.x * .9f, braking * .1f) : 0;
        if (isIdleY) speed.y = GetRealSpeed(speed.y) > speedDeadZone.y ? Mathf.Lerp(speed.y, speed.y * .9f, braking * .1f) : 0;
    }
}
