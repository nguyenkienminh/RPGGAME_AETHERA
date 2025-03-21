using UnityEngine;

public class ThunderStrike_Controller : MonoBehaviour
{
    [SerializeField] private CharacterStats targetStats;
    [SerializeField] private float speed;
    private int damage;
    private Animator animator;
    private bool triggered;
    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetupThunder(CharacterStats _targetStats, int _damage)
    {
        targetStats = _targetStats;
        damage = _damage;
    }
    private void Update()
    {
        if (!targetStats)
        {
            return;
        }


        if (triggered)
        {
            return;
        }

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, speed * Time.deltaTime);

        transform.right = transform.position - targetStats.transform.position;

        if(Vector2.Distance(transform.position, targetStats.transform.position) < .1f)
        {
            animator.transform.localPosition = new Vector3(0, .5f);
            animator.transform.localRotation = Quaternion.identity;

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3, 3);


            Invoke("DamageAndSelfDestroy", .2f);
            triggered = true;
            animator.SetTrigger("Hit");
        }
    }

    private void DamageAndSelfDestroy()
    {
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage);
        Destroy(gameObject, .4f);
    }

}
