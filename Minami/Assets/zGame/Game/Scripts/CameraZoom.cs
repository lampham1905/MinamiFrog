using UnityEngine;

public class CameraZoom : MonoBehaviour
{
        public float zoomSpeed = 0.5f;
    public float dragSpeed = 2f;
    public float minZoomDistance = 2f;
    public float maxZoomDistance = 10f;

    private Camera mainCamera;
    private Vector2? dragOrigin;
    private float initialCameraSize;

    void Start()
    {
        mainCamera = Camera.main;
        initialCameraSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        // Phóng to thu nhỏ
        HandlePinchZoom();

        // Kéo camera
        HandleDrag();
    }

    void HandlePinchZoom()
    {
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            ZoomCamera(-difference * zoomSpeed);
        }
    }

    void HandleDrag()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                dragOrigin = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved && dragOrigin != null)
            {
                Vector2 touchDelta = touch.position - (Vector2)dragOrigin;
                Vector3 newPosition = mainCamera.transform.position - new Vector3(touchDelta.x, touchDelta.y, 0f) * dragSpeed * Time.deltaTime;
                mainCamera.transform.position = newPosition;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                dragOrigin = null;
            }
        }
    }

    void ZoomCamera(float increment)
    {
        mainCamera.orthographicSize += increment;

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoomDistance, maxZoomDistance);
    }

}