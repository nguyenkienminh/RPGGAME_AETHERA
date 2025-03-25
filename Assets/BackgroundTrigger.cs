using UnityEngine;

public class BackgroundTrigger : MonoBehaviour
{
    private ManagerBackground manager;

    private void Start()
    {
        manager = GetComponentInParent<ManagerBackground>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            manager.ToggleBackground(); // Đổi trạng thái nền mỗi lần chạm
        }
    }
}
