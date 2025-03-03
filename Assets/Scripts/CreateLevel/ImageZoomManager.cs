using UnityEngine;

public class ImageZoomManager : MonoBehaviour
{
    [Header("Int & Float")]
    public float zoomSpeed = 0f;
    public float minZoom = 0f;
    public float maxZoom = 0f;

    public void Update()
    {
#if UNITY_EDITOR
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        Camera.main.orthographicSize -= scroll * zoomSpeed;
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
        Pan();

        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(touch1.position, touch2.position);
            float previousDistance = Vector2.Distance(touch1.position - touch1.deltaPosition, touch2.position - touch2.deltaPosition);
            float distanceDelta = previousDistance - currentDistance;

            Camera.main.orthographicSize += distanceDelta * zoomSpeed;
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minZoom, maxZoom);
        }
#endif

    }

    public void Pan()
    {
        if (Input.GetMouseButton(0))
        {
            float deltaX = Input.GetAxis("Mouse X");
            float deltaY = Input.GetAxis("Mouse Y");

            Vector3 panDirection = new Vector3(-deltaX, -deltaY, 0f) * 1 * Time.deltaTime;
            Camera.main.transform.Translate(panDirection, Space.Self);
        }
    }
}