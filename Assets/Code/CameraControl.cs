using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{


    float mainSpeed = 100.0f; //regular speed
    float shiftAdd = 250.0f; //multiplied by how long shift is held.  Basically running
    float maxShift = 1000.0f; //Maximum speed when holdin gshift
    private Vector3 lastMouse = new Vector3(255, 255, 255); //kind of in the middle of the screen, rather than at the top (play)
    public float scrollSpeed = 25f; //Speed of Scrolling on the Map
    private float totalRun = 1.0f; //How long the button was held down.
    public InputIO input; //Used only to confirm that we're supposed to allow the camera to move.
    void Update()
    {
        if (input.ConfirmHit)
        {
            //Keyboard Commands
            Vector3 p = GetBaseInput();
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                p = p * totalRun * shiftAdd;
                p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
                p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
                p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 0.5f, 1f, 1000f);
                p = p * mainSpeed;
            }

            p = p * Time.deltaTime;
            Vector3 newPosition = transform.position;
            transform.Translate(p);
            newPosition.x = transform.position.x;
            newPosition.z = transform.position.z;
            newPosition.y -= (Input.mouseScrollDelta.y * scrollSpeed);
            transform.position = checkBounds(newPosition);
        }
    }

    private Vector3 GetBaseInput()
    { //returns the basic values, if it's 0 than it's not active.
        Vector3 p_Velocity = new Vector3();
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            p_Velocity += new Vector3(0, 0, 1);
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            p_Velocity += new Vector3(0, 0, -1);
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            p_Velocity += new Vector3(-1, 0, 0);
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            p_Velocity += new Vector3(1, 0, 0);
        }
        return p_Velocity;
    }

    private float BoundMinY = 800 - 150, BoundMaxY = 800 + 150; //Base numbers
    public float topBoundMinX = -750, topBoundMinZ = -409, topBoundMaxX = -360, topBoundMaxZ = 360; //Numbers for if we're at BoundMinZ

    private Vector3 checkBounds(Vector3 pos)
    {
    if (pos.y > BoundMaxY)
    {
        pos.y = BoundMaxY;
    }

    else if (pos.y < BoundMinY)
    {
        pos.y = BoundMinY;
    }

    if (pos.x > topBoundMaxX)
    {
        pos.x = topBoundMaxX;
    }
    else if (pos.x < topBoundMinX)
    {
        pos.x = topBoundMinX;
    }

    if (pos.z > topBoundMaxZ)
    {
        pos.z = topBoundMaxZ;
    }
    else if (pos.z < topBoundMinZ)
    {
        pos.z = topBoundMinZ;
    }
        return pos;
    }
}