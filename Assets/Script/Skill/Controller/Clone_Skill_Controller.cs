using System.Collections;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{
    private Player player;
    private SpriteRenderer sr;
    private Animator animator;
    [SerializeField] private float colorLoosingSpeed;
    private float cloneTimer;
    private float attackMultiplier;
    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackCheckRadius = .8f;
    private bool canDuplicateClone;
    private int facingDir = 1;
    private float changeToDouplicate;

    [Space]
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private float closestEnemyCheckRadius = 25;
    [SerializeField] private Transform closestEnemy;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FaceClosesTarget());
    }
    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLoosingSpeed));

            if (sr.color.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
    public void SetupClone(Transform _newTransform, float _cloneDuration, bool _canAttack, Vector3 _offset, bool _canDuplicateClone, float _changeToDuplicate, Player _player, float _attackMultiplier)
    {
        if (_canAttack)
        {
            animator.SetInteger("AttackNumber", Random.Range(1, 3));
        }
        player = _player;
        transform.position = _newTransform.position + _offset;
        cloneTimer = _cloneDuration;

        canDuplicateClone = _canDuplicateClone;
        changeToDouplicate = _changeToDuplicate;
        attackMultiplier = _attackMultiplier;
    }
    private void FindClosesEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, closestEnemyCheckRadius, whatIsEnemy);

        float closestDistance = Mathf.Infinity;
        foreach (var hit in colliders)
        {

            float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

            if (distanceToEnemy < closestDistance)
            {
                closestDistance = distanceToEnemy;
                closestEnemy = hit.transform;
            }

        }
    }
    public void AnimationTrigger()
    {
        cloneTimer = -.1f;
    }

    private void AttackTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackCheckRadius);


        foreach (var hit in colliders)
        {

            if (hit.GetComponent<Enemy>() != null)
            {
                //player.chacracterStats.DoDamage(hit.GetComponent<CharacterStats>());
                hit.GetComponent<Entity>().SetupKnockbackDir(transform);

                PlayerStats playerStats = player.GetComponent<PlayerStats>();
                EnemyStats enemyStats = hit.GetComponent<EnemyStats>();

                if (playerStats == null)
                {
                    return;
                }

                if (enemyStats == null)
                {
                    return;
                }


                playerStats.CloneDoDamage(enemyStats, attackMultiplier);


                if (player.skill.cloneSkill.canApplyOnHitEffect)
                {
                    ItemData_Equipment weapon = Inventory.instance.GetEquipment(EquipmentType.Weapon);

                    if (weapon != null)
                    {
                        weapon.Effect(hit.transform);
                    }
                }

                if (canDuplicateClone)
                {
                    if (Random.Range(0, 100) < changeToDouplicate)
                    {
                        SkillManager.instance.cloneSkill.CreateClone(hit.transform, new Vector3(.5f * facingDir, 0));
                    }
                }
            }
        }
    }

    private IEnumerator FaceClosesTarget()
    {
        yield return null;

        FindClosesEnemy();

        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
            {
                facingDir = -1;
                transform.Rotate(0, 180, 0);
            }

        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, closestEnemyCheckRadius);
    }
}
