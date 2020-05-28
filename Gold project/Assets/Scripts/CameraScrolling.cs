using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    private Camera gameCamera;
    protected Plane Plane;

    [Header("The x boundaries of the camera")]
    public Vector2 cameraBounds;

    [Header("Options")]
    [Range(0f, 1f)] public float scrollSpeed = 1f;
    [Range(0f, 1f)] public float parallaxSpeed = 1f;

    [SerializeField] private List<Transform> parallax;

    public bool active;

    private Vector3 basePos;
    private Vector2 oriMousePos;

    private void Awake()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;

        basePos = gameCamera.transform.position;

        //for (int i = 0; i < cameraBounds.Length; i++)
        //{
        //    cameraBounds[i].y = gameCamera.transform.position.y;
        //    cameraBounds[i].z = gameCamera.transform.position.z;
        //}
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            oriMousePos = Input.mousePosition;

        if (Input.touchCount >= 1 && active)
        {
            Scroll();
        } else if(Input.GetMouseButton(0) && active)
        {
            float delta = -(Input.mousePosition.x - oriMousePos.x) * scrollSpeed;

            if (!CameraOutBounds(delta))
            {
                gameCamera.transform.Translate(new Vector2(delta, 0));
                Parallax(delta);
            } else
            {
                if (delta < 0)
                {
                    Parallax(cameraBounds.x - gameCamera.transform.position.x);
                    gameCamera.transform.position = new Vector3(cameraBounds.x, basePos.y, basePos.z);
                } else
                {
                    Parallax(cameraBounds.y - gameCamera.transform.position.x);
                    gameCamera.transform.position = new Vector3(cameraBounds.y, basePos.y, basePos.z);
                }
            }

            oriMousePos = Input.mousePosition;
        }
    }

    protected void Scroll()
    {
        Plane.SetNormalAndPosition(transform.up, transform.position);

        Vector3 Delta1 = Vector3.zero;
        Vector3 Delta2 = Vector3.zero;

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
        Ray rayBefore = gameCamera.ScreenPointToRay(touch.position - touch.deltaPosition);
        Ray rayNow = gameCamera.ScreenPointToRay(touch.position);
        if (Plane.Raycast(rayBefore, out float enterBefore) && Plane.Raycast(rayNow, out float enterNow))
        {
            ReplaceCameraInBounds();
            return new Vector3(rayBefore.GetPoint(enterBefore).x - rayNow.GetPoint(enterNow).x, 0, 0);
        }

        //not on plane
        return Vector3.zero;
    }

    protected void ReplaceCameraInBounds()
    {
            if (gameCamera.transform.position.x < cameraBounds.x)
            {
                gameCamera.transform.position = new Vector3(cameraBounds.x, basePos.y, basePos.z);
            }
            else if (gameCamera.transform.position.x > cameraBounds.y)
            {
                gameCamera.transform.position = new Vector3(cameraBounds.y, basePos.y, basePos.z);
            }
    }

    private bool CameraOutBounds(float movement)
    {
        if (gameCamera.transform.position.x + movement < cameraBounds.x || gameCamera.transform.position.x + movement > cameraBounds.y)
        {
            return true;
        }
        return false;
    }

    private void Parallax(float delta)
    {
        for (int i = 0; i < parallax.Count; i++)
        {
            parallax[i].Translate(new Vector2(delta * ((float)(parallax.Count - i) / (float)parallax.Count) * parallaxSpeed, 0));
        }
    }

    public void ToggleActive()
    {
        active = !active;
    }
}
