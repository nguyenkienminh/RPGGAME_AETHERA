using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float speed; 
    [SerializeField] private Transform PointA;
    [SerializeField] private Transform PointB;

    private Vector2 TargetPoint;

    private void Start()
    {
        TargetPoint = PointA.position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, PointA.position) < 0.1f)
        {
            TargetPoint = PointB.position;
        }
        else if (Vector2.Distance(transform.position, PointB.position) < 0.1f)
        {
            TargetPoint = PointA.position;
        }
        transform.position = Vector2.MoveTowards(transform.position, TargetPoint, speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(this.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Invoke(nameof(RemoveParent), 0.1f);
        }
    }

    private void RemoveParent()
    {
        if (transform.childCount > 0) 
        {
            transform.GetChild(0).SetParent(null);
        }
    }
}
