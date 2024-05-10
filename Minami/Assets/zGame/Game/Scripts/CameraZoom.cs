using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float dragSpeed = 2f; // Tốc độ di chuyển khi vuốt
    public float pinchSpeed = 2f; // Tốc độ phóng to/thu nhỏ khi pinch

    public float minOrthoSize = 2f; // Kích thước góc nhìn tối thiểu (phóng to nhất)
    public float maxOrthoSize = 10f; // Kích thước góc nhìn tối đa (thu nhỏ nhất)

    private Vector3 dragOrigin; // Vị trí ban đầu khi bắt đầu vuốt
    [SerializeField] private Camera mainCamera;
    private Vector3 newPos;
    void Update()
    {
// #if UNITY_EDITOR
//         // Kiểm tra xem người dùng có sử dụng chuột để kéo không
//         if (Input.GetMouseButtonDown(0))
//         {
//             dragOrigin = Input.mousePosition;
//             return;
//         }
//
//         if (!Input.GetMouseButton(0)) return;
//
//          newPos = mainCamera.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
//         Vector3 move = new Vector3(newPos.x * dragSpeed, 0, newPos.y * dragSpeed);
//
//         mainCamera.transform.Translate(move, Space.World);
//
//         // Kiểm tra xem người dùng có sử dụng con lăn chuột để phóng to/thu nhỏ không
//         float scroll = Input.GetAxis("Mouse ScrollWheel");
//         if (scroll != 0.0f)
//         {
//             Debug.Log("Scrolllllll");
//             mainCamera.orthographicSize -= scroll * pinchSpeed;
//             mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minOrthoSize, maxOrthoSize); // Giới hạn kích thước góc nhìn
//         }
// #endif
//#if UNITY_ANDROID || UNITY_IOS
        // Kiểm tra xem người dùng có vuốt trên màn hình không
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            // Lấy vị trí mới của ngón tay
            Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;

            // Tính toán vị trí mới của camera dựa trên vị trí mới của ngón tay và tốc độ di chuyển
            newPos = new Vector3(-touchDeltaPosition.x * dragSpeed * Time.deltaTime, 0, -touchDeltaPosition.y * dragSpeed * Time.deltaTime);

            // Di chuyển camera, nhưng chỉ thay đổi trục x và z, không thay đổi trục y
            mainCamera.transform.Translate(newPos, Space.World);
        }

        // Kiểm tra xem người dùng có pinch để phóng to/thu nhỏ không
        if (Input.touchCount == 2)
        {
            // Lấy vị trí hiện tại của cả hai ngón tay
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            // Lấy vị trí của các ngón tay ở frame trước
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Tính toán khoảng cách giữa các ngón tay ở frame trước và hiện tại
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Tính toán sự thay đổi tỉ lệ (phóng to/thu nhỏ)
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            // Áp dụng phóng to/thu nhỏ vào camera
            mainCamera.orthographicSize += deltaMagnitudeDiff * pinchSpeed * Time.deltaTime;
            mainCamera.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize, minOrthoSize, maxOrthoSize); // Giới hạn kích thước góc nhìn
        }
//#endif
       
    }

}