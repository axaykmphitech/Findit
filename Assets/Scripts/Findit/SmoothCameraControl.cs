using UnityEngine;

public class SmoothCameraControl : MonoBehaviour
{
    [Header("MainCamera")]
    public Camera camera;

    [Header("Int & Float")]
    public float zoomSpeed = 0;
    public float moveSpeed = 0;
    public float minZoom = 0;
    public float maxZoom = 0;
    public float minX = 0;
    public float maxX = 0;
    private float previousTouchDistance;

    void Update()
    {
        if (!UiManagerGame.Instance.isSelectCameraZoom)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Moved)
                {
                    if (camera.orthographicSize <= 10)
                    {
                        Vector3 delta = touch.deltaPosition;
                        camera.transform.Translate(-delta.x * moveSpeed * Time.deltaTime, -delta.y * moveSpeed * Time.deltaTime, 0);

                        Vector3 clampedPosition = camera.transform.position;
                        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
                        camera.transform.position = clampedPosition;
                    }
                }
            }
            else if (Input.touchCount == 2)
            {
                Touch touch0 = Input.GetTouch(0);
                Touch touch1 = Input.GetTouch(1);

                float currentTouchDistance = Vector2.Distance(touch0.position, touch1.position);

                if (touch0.phase == TouchPhase.Moved || touch1.phase == TouchPhase.Moved)
                {
                    float distanceDelta = currentTouchDistance - previousTouchDistance;

                    if (camera.orthographic)
                    {
                        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - distanceDelta * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
                    }
                    else
                    {
                        camera.fieldOfView = Mathf.Clamp(camera.fieldOfView - distanceDelta * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
                    }
                }
                previousTouchDistance = currentTouchDistance;
            }
        }
    }
}