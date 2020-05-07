using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    private Camera gameCamera;
    protected Plane Plane;

    [Header("The x boundaries of the camera")]
    public Vector3[] cameraBounds;

    private void Awake()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;

        for (int i = 0; i < cameraBounds.Length; i++)
        {
            cameraBounds[i].y = gameCamera.transform.position.y;
            cameraBounds[i].z = gameCamera.transform.position.z;
        }
    }

    private void Update()
    {
        if(Input.touchCount >= 1)
        {
            Scroll();
        }

    }

    protected void Scroll()
    {
        Plane.SetNormalAndPosition(transform.up, transform.position);

        var Delta1 = Vector3.zero;
        var Delta2 = Vector3.zero;

        Delta1 = PlanePositionDelta(Input.GetTouch(0));

        if (Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            gameCamera.transform.Translate(Delta1, Space.World);
        }
    }

    protected Vector3 PlanePositionDelta(Touch touch)
    {
        //not moved
        if (touch.phase == TouchPhase.Stationary)
        {
            return Vector3.zero;
        }


        //delta
        var rayBefore = gameCamera.ScreenPointToRay(touch.position - touch.deltaPosition);
        var rayNow = gameCamera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out var enterBefore) && Plane.Raycast(rayNow, out var enterNow))
        {
            ReplaceCameraInBounds();
            return new Vector3(rayBefore.GetPoint(enterBefore).x - rayNow.GetPoint(enterNow).x, 0, 0);
        }

        //not on plane
        return Vector3.zero;
    }

    protected void ReplaceCameraInBounds()
    {
            if (gameCamera.transform.position.x < cameraBounds[0].x)
            {
                gameCamera.transform.position = cameraBounds[0];
            }
            else if (gameCamera.transform.position.x > cameraBounds[1].x)
            {
                gameCamera.transform.position = cameraBounds[1];
            }
            else
            {
                gameCamera.transform.position = gameCamera.transform.position;
            }
    }
}
