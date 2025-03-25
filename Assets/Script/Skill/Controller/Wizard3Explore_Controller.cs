using System.Collections;
using UnityEngine;

public class Wizard3Explore_Controller : MonoBehaviour
{
    [SerializeField] private Transform check;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private LayerMask whatIsPlayer;
    private Vector3 targetPosition;
    private float speed = 25f;
    private CharacterStats myStats;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public void SetupSpell(CharacterStats stats)
    {
        myStats = stats;
    }

    public void MoveTowards(Vector3 target)
    {
        targetPosition = target;
        StartCoroutine(MoveToTarget());
    }

    private IEnumerator MoveToTarget()
    {
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        Explode();
    }

    private void Explode()
    {
        AnimationTrigger();
        Destroy(gameObject);
    }

    private void AnimationTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapBoxAll(check.position, boxSize, 0, whatIsPlayer);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Player>() != null)
            {
                AudioManager.instance.PlaySFX(42, transform);
                PlayerStats target = hit.GetComponent<PlayerStats>();
                myStats.DealTotalDamage(target, false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Explode(); // Nổ khi chạm vào player
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(check.position, boxSize);
    }
    private void SelfDestroy() => Destroy(gameObject);
}

