using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main; // Lấy camera chính
    }

    void LateUpdate()
    {
        // Luôn quay mặt về camera
        transform.forward = mainCamera.transform.forward;
    }
}
