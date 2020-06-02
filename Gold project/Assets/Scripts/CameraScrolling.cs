using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    private Camera gameCamera;
    protected Plane Plane;

    [Header("The x boundaries of the camera")]
    //public Vector2 cameraBounds;
    public SpriteRenderer cameraBounds;
    private Vector2 bounds;

    [Header("Options")]
    [Range(0f, 1f)] public float scrollSpeed = 1f;
    [Range(0f, 1f)] public float parallaxSpeed = 1f;

    [SerializeField] private List<Transform> parallax;

    public bool active;

    private Vector3 basePos;
    private float oriMousePosX;

    private Vector2 cameraSize;

    private void Awake()
    {
        if (gameCamera == null)
            gameCamera = Camera.main;

        basePos = gameCamera.transform.position;

        float orthoSize = cameraBounds.bounds.size.y / 2;
        gameCamera.orthographicSize = orthoSize;

        cameraSize = new Vector2(orthoSize / 0.5f * Screen.width / Screen.height, orthoSize);
        Debug.Log(cameraSize);

        bounds = new Vector2(cameraBounds.transform.position.x - cameraBounds.bounds.size.x/2, cameraBounds.transform.position.x + cameraBounds.bounds.size.x/2);
    }

    private void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            oriMousePosX = Input.GetTouch(0).position.x;
        else if (Input.GetMouseButtonDown(0))
            oriMousePosX = Input.mousePosition.x;

        if (Input.touchCount >= 1 && active)
            Scroll(Input.GetTouch(0).position.x);
        else if(Input.GetMouseButton(0) && active)
            Scroll(Input.mousePosition.x);
    }

    private void Scroll(float posX)
    {
        float delta = -(posX - oriMousePosX) * scrollSpeed;

        if (!CameraOutBounds(delta))
        {
            gameCamera.transform.Translate(new Vector2(delta, 0));
            Parallax(delta);
        }
        else
        {
            if (delta < 0)
            {
                Parallax(bounds.x - (gameCamera.transform.position.x - cameraSize.x / 2));
                gameCamera.transform.position = new Vector3(bounds.x + cameraSize.x / 2, basePos.y, basePos.z);
            }
            else
            {
                Parallax(bounds.y - (gameCamera.transform.position.x + cameraSize.x / 2));
                gameCamera.transform.position = new Vector3(bounds.y - cameraSize.x / 2, basePos.y, basePos.z);
            }
        }

        oriMousePosX = posX;
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
            //ReplaceCameraInBounds();
            return new Vector3(rayBefore.GetPoint(enterBefore).x - rayNow.GetPoint(enterNow).x, 0, 0);
        }

        //not on plane
        return Vector3.zero;
    }

    private bool CameraOutBounds(float movement)
    {
        if (gameCamera.transform.position.x - cameraSize.x / 2 + movement < bounds.x ||
            gameCamera.transform.position.x + cameraSize.x / 2 + movement > bounds.y
        )
            return true;

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
